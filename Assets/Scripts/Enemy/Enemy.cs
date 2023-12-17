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
using UnityEngine.Tilemaps;
using System.Runtime.InteropServices;

[System.Serializable]
public class AudioDelaySettings
{
    [Range(0f, 1f)] public float attacking_1 = 0.2f;
    [Range(0f, 1f)] public float attacking_2 = 0.2f;
    [Range(0f, 1f)] public float take_hit = 0.2f;
    [Range(0f, 1f)] public float death = 0.2f;

}

[System.Serializable]
public class BuffEvent : UnityEvent<Enemy>
{
}

// custom class for enemy
public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]

    public int life = 1;

    public float maxHealth;
    private float health;

    public float Health {
        get {
            return health;
        }
    }

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
    private float poisonAffectInterval = 1.5f;

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

    [SerializeField] private BuffEvent surviveEvent;
    [SerializeField] private BuffEvent attackEvent;

    [SerializeField] private BuffEvent deathEvent;

    private int ID;

    private Castle castle;

    private Transform castleGround;

    private int PathIndex;

    private Transform nextPath;

    private GameObject player;

    // getter and setter
    public GameObject Player {
        get {
            return player;
        }
    }

    private HPControl playerHP;

    private Transform playerGround;

    private int actionMode;

    public int ActionMode {
        get {
            return actionMode;
        }
        set {
            actionMode = value;
        }
    }

    private Animator animator;

    private string isAttacking = "IsAttacking";
    private string isAttacked = "IsAttacked";
    private string isDead = "IsDead";

    private string isWalking = "IsWalking";

    private string isCrit = "IsCrit";

    // private string isMoving = "IsMoving";

    private bool inAttackInterval;

    private EnemyAudio enemyAudio;

    private string enemyName;

    private bool isResurrected = false;

    public bool IsResurrected {
        get {
            return isResurrected;
        }
        set {
            isResurrected = value;
        }
    }

    private Tilemap ground;

    public void Init(int enemyID, Vector3? position = null)
    {
        PathIndex = 1;

        maxHealth *= Difficulty.enemyHealthRate;
        life += Difficulty.enemyAdditionLife;
        damage *= Difficulty.enemyDamageRate;

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

        // find tilemap ground
        GameObject grid = GameObject.Find("Grid");
        ground = grid.transform.Find("Ground").GetComponent<Tilemap>();

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
        if (position == null) {
            transform.position = LevelManager.Instance.GetPathLocations()[0].position;

            nextPath = LevelManager.Instance.GetPathLocations()[PathIndex];
        } else {
            transform.position = (Vector3)position;
            actionMode = 0;
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

    public Vector3 GetOnGround(Vector3 position) {
        Vector3Int cellPosition = ground.WorldToCell(position);

        cellPosition = new Vector3Int(Math.Min(cellPosition.x, 10), Math.Min(cellPosition.y, 10), cellPosition.z);
        cellPosition = new Vector3Int(Math.Max(cellPosition.x, -14), Math.Max(cellPosition.y, -14), cellPosition.z);


        // Debug.Log(cellPosition);
        // change to world position
        Vector3 worldPosition = ground.CellToWorld(cellPosition) - new Vector3(0.5f, 0.5f, 0f);
        return worldPosition;
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

    public void DeductHealth(float damage, float percentHPDamage = 0f) {
        if (actionMode == -2) {
            return;
        }
        damage += percentHPDamage * health;
        //Debug.Log(percentHPDamage);
        health -= damage;
        healthBar.value = health / maxHealth;

        animator.SetTrigger(isAttacked);
        enemyAudio?.take_hit.PlayDelayed(delayTime.take_hit);

        hurtUI.Init(damage, transform, false);

        // animator.Play("take_hit", -1, 0f);
        if (health <= 0) {
            life--;
            if (life > 0) {
                health += maxHealth;
                return;
            }
            deathEvent?.Invoke(this);
            enemyAudio?.death.PlayDelayed(delayTime.death);
            StartCoroutine(DeathAnimation());
        }
    }

    public void RecoverHealth(float health, float percent = 0) {
        this.health += health + percent * maxHealth;;
        if (this.health > maxHealth) {
            this.health = maxHealth;
        }
        healthBar.value = this.health / maxHealth;
    }

    public void DeductHealthPercent(float percent) {
        DeductHealth(percent * health);
    }

    public void GiveEnemyDebuff(int id, float time, String debuffName, float debuffValue) {
        bool idExist = false;

        for(int i = Debuffs.Count - 1; i >= 0; i--){
            if (Debuffs[i].id == id){
                if (id == 3) {
                    idExist = true;
                    break;
                }
                if (DebuffTimers[i] < time){
                    DebuffTimers[i] = time;
                    /*Debuff newdebuff = Debuffs[i];
                    newdebuff.timer = time;
                    Debuffs[i] = newdebuff;*/
                    idExist = true;
                    break;
                }
            }
        }

        if(!idExist){
            Debuffs.Add(new Debuff(id, debuffName, debuffValue));
            DebuffTimers.Add(time);
            if (debuffName == "slow"){
                speed *= debuffValue;
            }
            else if (debuffName == "reduceMaxHP"){
                health *= debuffValue;
                maxHealth *= debuffValue;
                healthBar.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color (0.6f, 0 , 1f);
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
                    if (Debuffs[i].debuffValue < 0.1) {
                        Debuffs.Add(new Debuff(Debuffs[i].id, Debuffs[i].debuffName, 1));
                        DebuffTimers.Add(1);
                    }
                }
                else if (Debuffs[i].debuffName == "reduceMaxHP"){
                    health /= Debuffs[i].debuffValue;
                    maxHealth /= Debuffs[i].debuffValue;
                    healthBar.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.red;
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

        if (!isResurrected) {
            LevelManager.Instance.EnqueEnemyToRemove(this);
        } 
        yield return null;
    }

    public void AssignNearestPath() {
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
        if (transform.position == null){
            Debug.Log(1);
        }
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
            attackEvent?.Invoke(this);
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
            if (Vector3.Distance(transform.position, nextPath.position) <= 0.3f)
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
        surviveEvent?.Invoke(this);
        if (player != null) {
            if (actionMode == -2) {
                // do nothing
            } 
            else if (Vector3.Distance(transform.position, castleGround.position) <= 2 && !inAttackInterval) {
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