using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

// custom class for enemy
public class Enemy : MonoBehaviour
{
    public float maxHealth;
    private float health;
    public float damage;
    public float speed;

    public float attackRange;

    public float detectRange;

    private int ID;

    public Slider healthBar;

    private int PathIndex;

    private Transform nextPath;

    private GameObject player;

    private Transform playerGround;

    private int actionMode;

    private Animator animator;

    private string isAttacking = "IsAttacking";
    private string isAttacked = "IsAttacked";
    private string isDead = "IsDead";

    public void Init(int enemyID)
    {
        PathIndex = 1;
        health = maxHealth;
        ID = enemyID;
        healthBar.value = health / maxHealth;

        actionMode = 1;

        // search for the player
        player = GameObject.Find("Player");

        // playerground is the transform of the player's child when name "GroundSensor"
        playerGround = player.transform.Find("GroundSensor");

        // get the animator in the first child
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();

        // set isAttacking, isAttacked, isDead to false
        animator.SetBool(isAttacking, false);
        animator.SetBool(isDead, false);

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

        // set enemy to the first path location
        transform.position = LevelManager.Instance.PathLocations[0].position;

        nextPath = LevelManager.Instance.PathLocations[PathIndex];
    }
    public int GetID() {
        return ID;
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

    private IEnumerator DeathAnimation() {
        animator.SetBool(isDead, true);
        yield return new WaitForSeconds(1);
        animator.SetBool(isDead, false);
        EnemySummon.RemoveEnemy(this);
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
        GameObject Skin = transform.GetChild(0).gameObject;
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

        Debug.Log("Enemy.cs: AttackPlayer() called.");

        animator.SetBool(isAttacking, true);
        // animator.SetTrigger(isAttacking);

        // if (Vector3.Distance(transform.position, player.transform.position) <= attackRange) {
        //     // deal damage to player
        //     player.GetComponent<Player>().DeductHealth(damage);
        // }

        // if player is dead, end attacking
        // if (player.GetComponent<Player>().GetHealth() <= 0) {
        //     actionMode = 0;
        // }
    }

    private void ChasePlayer() {
        if (actionMode == 1) {
            // end moving to target
            actionMode = 0;
        } else if (actionMode == -1) {
            // end attacking
            actionMode = 0;
            animator.SetBool(isAttacking, false);

        }
        
        Move(playerGround);
    }

    private void MoveToPath() {
        if (actionMode == 0) {
            // end chasing player
            AssignNearestPath();
            actionMode = 1;
        }
        Move(nextPath);

        if (Vector3.Distance(transform.position, nextPath.position) <= 0.1f)
        {
            PathIndex++;

            if (PathIndex == LevelManager.Instance.PathLocations.Count)
            {
                // if the enemy reaches the end of the path, deal damage to the player

                // send itself to the queue to be removed
                EnemySummon.RemoveEnemy(this);
            }
            else
            {
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

        if (player != null) {
            if (Vector3.Distance(transform.position, playerGround.position) <= attackRange) {
                AttackPlayer();
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