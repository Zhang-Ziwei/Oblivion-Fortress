using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public float attackRange = 5;
    public float Damage = 1F;
    public string Tag = "Enemy";
    public float attackInterval = 1f;
    public string attackType = "Single";
    private float timePassed = 0f;
    private LineRenderer line;
    private GameObject area; 

    void Start()
    {
        if (attackType == "Single")
        {
            line = gameObject.GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, transform.position + new Vector3(0,0.5f,0));
            line.SetPosition(1, transform.position + new Vector3(0,0.5f,0));
        }
        else if (attackType == "Area")
        {
            area = gameObject.gameObject.transform.GetChild(1).gameObject;
            area.transform.localScale = new Vector3(attackRange, attackRange, 1);
        }
    }

    void A(){
        if (attackType == "Single")
        {
            line.SetPosition(1, transform.position + new Vector3(0,0.5f,0));
        }
        else if (attackType == "Area")
        {
            area.SetActive(false);
        }
    }

    /*void OnMouseOver(){
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
    }

    void OnMouseExit(){
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
    }*/

    void Update()
    {
        
        timePassed += Time.deltaTime;
        if(timePassed > attackInterval)
        {
            if (attackType == "Single")
            {
                GameObject nearestEnemy = GameData.getNearestObjectWithTag(transform.position, Tag);
                if(nearestEnemy && GameData.distanceRec(transform.position, nearestEnemy.transform.position) < attackRange)
                {
                    nearestEnemy.GetComponent<Enemy>().DeductHealth(Damage);
                    line.SetPosition(1, nearestEnemy.transform.position + new Vector3(0,0.5f,0));
                    Invoke("A", 0.1f);
                }
            }

            else if (attackType == "Area")
            {
                GameObject[] enemies = GameData.getInRangeObjectWithTag(transform.position, Tag, attackRange);
                if(enemies.Length > 0)
                {
                    foreach(GameObject enemy in enemies)
                    {
                        enemy.GetComponent<Enemy>().DeductHealth(Damage);
                    }
                    area.SetActive(true);
                    Invoke("A", 0.1f);
                }
            }
            timePassed = 0f;
        } 
    }
}
