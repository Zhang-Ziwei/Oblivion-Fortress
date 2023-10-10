using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

// custom class for enemy
public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float damage;
    public float speed;
    public int ID;

    public Slider healthBar;

    private Vector3 DefaultDirection = new Vector3(-1, -0.5f, 0).normalized;

    private int PathIndex = 1;

    private Transform target;

    public void Init(int enemyID)
    {
        maxHealth = 5;
        health = maxHealth;
        ID = enemyID;
        healthBar.value = health / maxHealth;

        target = LevelManager.Instance.PathLocations[PathIndex];
    }

    void Start()
    {
        // healthbar direction 
        healthBar.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {

        Vector2 direction = (target.position - transform.position).normalized;

        // Vector2 direction = new Vector2(-100, 0).normalized;

        // move towards the target
        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
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
                target = LevelManager.Instance.PathLocations[PathIndex];
            }
        }

    }
    public void DeductHealth(float damage) {
        health -= damage;
        healthBar.value = health / maxHealth;
        if (health <= 0) {
            EnemySummon.RemoveEnemy(this);
        }
    }
}