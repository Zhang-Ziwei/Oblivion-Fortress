using System.Collections;
using System.Collections.Generic;
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
    private int ID;

    public Slider healthBar;

    private Vector3 DefaultDirection = new Vector3(-1, -0.5f, 0).normalized;

    private int PathIndex;

    private Transform target;

    public void Init(int enemyID)
    {
        PathIndex = 1;
        health = maxHealth;
        ID = enemyID;
        healthBar.value = health / maxHealth;

        // if maxhealth == 0, debug log
        if (maxHealth == 0)
        {
            Debug.Log("Enemy.cs: maxHealth is 0.");
        }
        
        // set enemy to the first path location
        transform.position = LevelManager.Instance.PathLocations[0].position;

        target = LevelManager.Instance.PathLocations[PathIndex];
    }

    void Start()
    {
        // healthbar direction 
        healthBar.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public int GetID() {
        return ID;
    }

    void Update()
    {

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