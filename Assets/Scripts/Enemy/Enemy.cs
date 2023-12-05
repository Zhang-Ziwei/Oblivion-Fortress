using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;

//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using TMPro;

[System.Serializable]
public class AudioDelaySettings
{
    [Range(0f, 1f)] public float attacking_1 = 0.2f;
    [Range(0f, 1f)] public float attacking_2 = 0.2f;
    [Range(0f, 1f)] public float take_hit = 0.2f;
    [Range(0f, 1f)] public float death = 0.2f;

}

// custom class for enemy
public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]

    public float maxHealth;
    private float health;
    public float damage;
    public float speed;

    private float originSpeed;
    private struct Debuff
    {
        public int id;
        public String debuffName;
        public float debuffValue;
        public Debuff(int id, String debuffName, float debuffValue)
        {
            this.id = id;
            this.debuffName = debuffName;
            this.debuffValue = debuffValue;
        }
    };
    private List<Debuff> Debuffs = new List<Debuff>();
    private List<float> DebuffTimers = new List<float>(); 
    private float poisonTimer = 0f;
    private float poisonAffectInterval = 0.5f;

    public float attackRange = 1;

    public float detectRange = 2f;

    public float attackInterval = 1f;

    [Range(0f, 1f)] public float critChance = 0.1f;

    [Range(1f, 3f)] public float critMultiplier = 2f;

    [SerializeField] private Slider healthBar;

    [SerializeField] private HurtUI hurtUI;

    [Header("Audio")]

    [Range(0f, 2f)] public float audioSpeed = 1f;

    [Range(0f, 1f)] public float volume = 0.2f;

    public AudioDelaySettings delayTime;

    [Header("Enemy Buffs")]

    public EnemyBuff[] enemyBuffs;

    private int ID;

    private Castle castle;

    private Transform castleGround;

    private int PathIndex;

    private Transform nextPath;

    private GameObject player;

    private HPControl playerHP;

    private Transform playerGround;

    private int actionMode;

    private Animator animator;

    private string isAttacking = "IsAttacking";
    private string isAttacked = "IsAttacked";
    private string isDead = "IsDead";

    private string isWalking = "IsWalking";

    private string isCrit = "IsCrit";

    // private string isMoving = "IsMoving";

    private bool inAttackInterval;

    private List<UnityEvent> attackEvents;

    private EnemyAudio enemyAudio;

    private string enemyName;

    public void Init(int enemyID)
    {
        PathIndex = 1;
        health = maxHealth;
        ID = enemyID;
        healthBar.value = health / maxHealth;
        originSpeed = speed;

        actionMode = 1;

        inAttackInterval = false;

        // search for the player
        player = GameObject.Find("Player");

        // playerground is the transform of the player's child when name "GroundSensor"
        playerGround = player.transform.Find("GroundSensor");

        // get the animator in the child named "Skin"
        animator = transform.Find("Skin").GetComponent<Animator>();

        // find the castle
        castle = GameObject.Find("castle").GetComponent<Castle>();

        // find the player HP controller
        playerHP = GameObject.Find("Player").GetComponent<HPControl>();

        // find castle ground
        castleGround = castle.transform.Find("GroundSensor");

        // get the corresponding prefab name
        GameObject prefab = EnemySummon.EnemyPrefabs[enemyID];
        enemyName = prefab.name;

        // if maxhealth == 0, debug log
        if (maxHealth == 0)
        {
            Debug.Log("Enemy.cs: maxHealth is 0.");
        }
        
        // if attack range > detect range, debug log
        if (attackRange > detectRange)
        {
            Debug.Log("Enemy.cs: attackRange is larger than detectRange.");
        }

        if (critChance > 1 || critChance < 0) {
            Debug.Log("Enemy.cs: critChance error.");
        }

        animator.SetBool(isWalking, false);
        animator.SetFloat(isCrit, 0);

        // set enemy to the first path location
        transform.position = LevelManager.Instance.GetPathLocations()[0].position;

        nextPath = LevelManager.Instance.GetPathLocations()[PathIndex];

        // add all enemy buffs to attackEvents
        attackEvents = new List<UnityEvent>();
        foreach (EnemyBuff enemyBuff in enemyBuffs) {
            if (enemyBuff != null) {
                enemyBuff.Init();

                UnityEvent nowEvent = new UnityEvent();
                nowEvent.AddListener(enemyBuff.Buff);
                attackEvents.Add(nowEvent);
            }
            
        }

        // load audio
        if (LevelManager.Instance.enemyAudios.ContainsKey(enemyName))
        {
            enemyAudio = LevelManager.Instance.enemyAudios[enemyName];
            enemyAudio?.ApplySettings(volume, audioSpeed, false);
        }
    }
    public int GetID() {
        return ID;
    }

    // decide whether the enemy is crit or not
    public int SetIsCrit(float damage) {
        float random = Random.Range(0f, 1f);
        int nowIsCrit = 0;
        if (random <= critChance) {
            nowIsCrit = 1;
            enemyAudio?.attacking_2.PlayDelayed(delayTime.attacking_2);
            playerHP.DeductHP(damage * critMultiplier, true, delayTime.attacking_2);
        } else {
            nowIsCrit = 0;
            playerHP.DeductHP(damage, false, delayTime.attacking_1);
            enemyAudio?.attacking_1.PlayDelayed(delayTime.attacking_1);
        }
        animator.SetFloat(isCrit, nowIsCrit);
        return nowIsCrit;
    }

    public void DeductHealth(float damage) {
        health -= damage;
        healthBar.value = health / maxHealth;

        animator.SetTrigger(isAttacked);
        enemyAudio?.take_hit.PlayDelayed(delayTime.take_hit);

        hurtUI.Init(damage, transform, false);

        // animator.Play("take_hit", -1, 0f);
        if (health <= 0) {
            enemyAudio?.death.PlayDelayed(delayTime.death);
            StartCoroutine(DeathAnimation());
        }
    }

    public void DeductHealthPercent(float percent) {
        DeductHealth(percent * maxHealth);
    }

    public void GiveEnemyDebuff(int id, float time, String debuffName, float debuffValue) {
        bool idExist = false;

        for(int i = Debuffs.Count - 1; i >= 0; i--){
            if (Debuffs[i].id == id){
                if (DebuffTimers[i] < time){
                    DebuffTimers[i] = time;
                    /*Debuff newdebuff = Debuffs[i];
                    newdebuff.timer = time;
                    Debuffs[i] = newdebuff;*/
                    idExist = true;
                }
            }
        }

        if(!idExist){
            Debuffs.Add(new Debuff(id, debuffName, debuffValue));
            DebuffTimers.Add(time);
            if (debuffName == "slow"){
                speed *= debuffValue;
            }
        }
    }

    private void UpdateDebuff(){
        poisonTimer += Time.deltaTime;
        for(int i = Debuffs.Count - 1; i >= 0; i--){
            DebuffTimers[i] -= Time.deltaTime;
            if (poisonTimer >= poisonAffectInterval && Debuffs[i].debuffName == "poison"){
                DeductHealth(Debuffs[i].debuffValue);
            }
            if (DebuffTimers[i] <= 0) {
                if (Debuffs[i].debuffName == "slow"){
                    speed /= Debuffs[i].debuffValue;
                }
                Debuffs.RemoveAt(i);
                DebuffTimers.RemoveAt(i);
            }
        }
        if (poisonTimer >= poisonAffectInterval) poisonTimer = 0f;
    }

    private IEnumerator AttackAnimation() {
        inAttackInterval = true;
        animator.SetTrigger(isAttacking);

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(isAttacking)) {
            yield return null;
        }

        yield return new WaitForSeconds(attackInterval);
        inAttackInterval = false;
        yield return null;
    }

    private IEnumerator DeathAnimation() {
        animator.SetTrigger(isDead);
        // while (animator.GetCurrentAnimatorStateInfo(0).IsName(isDead)) {
        //     yield return new WaitForSeconds(0.1f);
        // }

        yield return new WaitForSeconds(1f);

        LevelManager.Instance.EnqueEnemyToRemove(this);
        yield return null;
    }

    private void AssignNearestPath() {
        // return the nearest path location in LevelManager.Instance.PathLocations
        Transform nearestPath = LevelManager.Instance.GetPathLocations()[0];
        float minDistance = Vector3.Distance(transform.position, nearestPath.position);
        int minIndex = 0;
        for (int i = 1; i < LevelManager.Instance.GetPathLocations().Count; i ++) {
            float distance = Vector3.Distance(transform.position, LevelManager.Instance.GetPathLocations()[i].position);
            if (distance < minDistance) {
                nearestPath = LevelManager.Instance.GetPathLocations()[i];
                minDistance = distance;
                minIndex = i;
            }
        }
        PathIndex = minIndex;
        nextPath = nearestPath;
    }

    private void Move(Transform target) {

        Vector2 direction = (target.position - transform.position).normalized;
        // if x is negative, flip the sprite of the child object
        GameObject Skin = transform.Find("Skin").gameObject;

        animator.SetBool(isWalking, true);

        // if y is negative, layer order is -1; else, layer order is 1
        if (direction.y < 0)
        {
            Skin.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        else
        {
            Skin.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        if (direction.x < 0)
        {
            Skin.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            Skin.GetComponent<SpriteRenderer>().flipX = false;
        }
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void AttackPlayer() {
        if (actionMode == 0) {
            // end chasing player
            actionMode = -1;
        } else if (actionMode == 1) {
            // end moving to target
            actionMode = -1;
        }
        animator.SetBool(isWalking, false);
        int nowIsCrit = SetIsCrit(damage);
        StartCoroutine(AttackAnimation());

        // invoke all enemy buffs
        if (nowIsCrit == 1) {
            foreach (UnityEvent attackEvent in attackEvents) {
                attackEvent?.Invoke();
            }
        }

    }

    private void AttackCastle() {
        if (actionMode == 0) {
            // end chasing player
            actionMode = -1;
        } else if (actionMode == 1) {
            // end moving to target
            actionMode = -1;
        }

        StartCoroutine(AttackAnimation());
        castle.DeductHealth(damage);
    }

    private void ChasePlayer() {
        if (actionMode == 1) {
            // end moving to target
            actionMode = 0;
        } else if (actionMode == -1) {
            // end attacking
            actionMode = 0;
        }
        // if the enemy is near the player, stop moving
        if (Vector3.Distance(transform.position, playerGround.position) <= attackRange) {
            return;
        } else {
            Move(playerGround);
        }
    }

    private void MoveToPath() {
        if (actionMode == 0) {
            // end chasing player
            AssignNearestPath();
            actionMode = 1;
        }
        if (PathIndex < LevelManager.Instance.GetPathLocations().Count - 1)
        {
            Move(nextPath);
            if (Vector3.Distance(transform.position, nextPath.position) <= 0.1f)
            {
                PathIndex++;
                nextPath = LevelManager.Instance.GetPathLocations()[PathIndex];
            } 
        }
    }

    void Start()
    {
        // healthbar direction 
        healthBar.transform.rotation = Quaternion.Euler(0, 0, 0);
        // StartCoroutine(Test());
    }

    void Update()
    {
        UpdateDebuff();

        if (player != null) {
            if (Vector3.Distance(transform.position, castleGround.position) <= 2 && !inAttackInterval) {
                AttackCastle();
                // Debug.Log("Enemy.cs: AttackCastle() called.");
    
            } else if (Vector3.Distance(transform.position, playerGround.position) <= attackRange && !inAttackInterval && (playerHP.HP > 0)) {
                AttackPlayer();
                // Debug.Log("Enemy.cs: AttackPlayer() called.");
            } else if (Vector3.Distance(transform.position, playerGround.position) <= detectRange && (playerHP.HP > 0)) {
                ChasePlayer();
            } else {
                MoveToPath();
            }
        }    
    }
}