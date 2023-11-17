using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;

//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;


// custom class for enemy
public class Enemy : MonoBehaviour
{
    public float maxHealth;
    private float health;
    public float damage;
    public float speed;

    private float originSpeed;
    private float SlowDownTimer;

    public float attackRange;

    public float detectRange;

    public float attackInterval;

    public float critChance;

    public Slider healthBar;

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

    private EnemyBuff[] enemyBuffs;

    private List<UnityEvent> attackEvents;

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
        transform.position = LevelManager.Instance.PathLocations[0].position;

        nextPath = LevelManager.Instance.PathLocations[PathIndex];


        // find all enemy buffs attach to this enemy
        enemyBuffs = GetComponents<EnemyBuff>();

        // add all enemy buffs to attackEvents
        attackEvents = new List<UnityEvent>();
        foreach (EnemyBuff enemyBuff in enemyBuffs) {
            attackEvents.Add(enemyBuff.NowEvent);
        }
    }
    public int GetID() {
        return ID;
    }

    // decide whether the enemy is crit or not
    public void SetIsCrit() {
        float random = Random.Range(0f, 1f);
        if (random <= critChance) {
            animator.SetFloat(isCrit, 1);
        } else {
            animator.SetFloat(isCrit, 0);
        }
    }

    public void DeductHealth(float damage) {
        health -= damage;
        healthBar.value = health / maxHealth;

        animator.SetTrigger(isAttacked);
        // animator.Play("take_hit", -1, 0f);
        if (health <= 0) {
            StartCoroutine(DeathAnimation());
        }
    }

    public void SlowDownEnemy(float rate, float seconds) {
        speed = rate * originSpeed;
        SlowDownTimer = seconds;
    }

    private IEnumerator AttackAnimation() {
        inAttackInterval = true;
        SetIsCrit();
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
        Transform nearestPath = LevelManager.Instance.PathLocations[0];
        float minDistance = Vector3.Distance(transform.position, nearestPath.position);
        int minIndex = 0;
        for (int i = 1; i < LevelManager.Instance.PathLocations.Count; i ++) {
            float distance = Vector3.Distance(transform.position, LevelManager.Instance.PathLocations[i].position);
            if (distance < minDistance) {
                nearestPath = LevelManager.Instance.PathLocations[i];
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

        StartCoroutine(AttackAnimation());
        playerHP.DeductHP(damage);

        // invoke all enemy buffs
        foreach (UnityEvent attackEvent in attackEvents) {
            attackEvent.Invoke();
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
        if (PathIndex < LevelManager.Instance.PathLocations.Count - 1)
        {
            Move(nextPath);
            if (Vector3.Distance(transform.position, nextPath.position) <= 0.1f)
            {
                PathIndex++;
                nextPath = LevelManager.Instance.PathLocations[PathIndex];
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
        if (SlowDownTimer > 0) {
            SlowDownTimer -= Time.deltaTime;
            if (SlowDownTimer <= 0) {
                speed = originSpeed;
            }
        }
        if (player != null) {
            if (Vector3.Distance(transform.position, castleGround.position) <= 2 && !inAttackInterval) {
                AttackCastle();
                // Debug.Log("Enemy.cs: AttackCastle() called.");
    
            } else if (Vector3.Distance(transform.position, playerGround.position) <= attackRange && !inAttackInterval) {
                AttackPlayer();
                // Debug.Log("Enemy.cs: AttackPlayer() called.");
            } else if (Vector3.Distance(transform.position, playerGround.position) <= detectRange) {
                ChasePlayer();
            } else {
                MoveToPath();
            }
        }    
    }
    IEnumerator Test() {
        while(true) {
            yield return new WaitForSeconds(3);
            DeductHealth(1);
        }
    }
}