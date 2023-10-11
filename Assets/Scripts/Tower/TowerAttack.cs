using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public float attackRange = 5;
    public float Damage = 0.1F;
    public string Tag = "Enemy";
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        GameObject nearestEnemy = GameData.getNearestObjectWithTag(transform.position, Tag);
        if(nearestEnemy && GameData.distanceRec(transform.position, nearestEnemy.transform.position) < attackRange){
            nearestEnemy.GetComponent<Enemy>().DeductHealth(Damage);
        }
    }
}
