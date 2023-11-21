using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tower : MonoBehaviour
{
    public float attackRange = 5;
    public float Damage = 1F;
    public float slowDownRate = 0.5f;
    public float slowDownTime = 5;
    public string Tag = "Enemy";
    public float attackInterval = 1f;
    public string attackType = "Single";
    private float timePassed = 0f;

    // used for attack animation
    private LineRenderer line;
    private GameObject area;
    private GameObject lightningEnd;
    private Vector3 lightningEndAtStart;
    private GameObject arrow; 

    private GameObject preAttackEnemy = null;

    public GameObject upgradeBase1;
    public GameObject upgradeBase2;


    void BuffTower(String buffName, float buffRate, float buffTime){
        if (buffName == "damage"){
            Damage = Damage * buffRate;
            StartCoroutine(RestoreBuff(buffName, buffRate, buffTime));
        }
    }

    IEnumerator RestoreBuff(String buffName, float buffRate, float buffTime)
    {
        yield return new WaitForSeconds(buffTime);
        if (buffName == "damage"){
            Damage = Damage / buffRate;
        }
    }

    void Start()
    {
        if (attackType == "Single")
        {
            //line = gameObject.GetComponent<LineRenderer>();
            //line.positionCount = 2;
            //line.SetPosition(0, transform.position + new Vector3(0,0.5f,0));
            //line.SetPosition(1, transform.position + new Vector3(0,0.5f,0));
            arrow = gameObject.transform.GetChild(2).gameObject; 
        }
        else if (attackType == "Area" || attackType == "AreaSlow" || attackType == "AreaPoison" || attackType == "AreaRecover" || attackType == "AreaBuff")
        {
            area = gameObject.transform.GetChild(2).gameObject;
            //area.transform.localScale = new Vector3(attackRange, attackRange, 1);
        }
        else if (attackType == "Single_L" || attackType == "Single_L2")
        {
            lightningEnd = gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject;
            lightningEndAtStart = lightningEnd.transform.position;
        }
    }

    void A(){
        if (attackType == "Single")
        {
            //line.SetPosition(1, transform.position + new Vector3(0,0.5f,0));
            arrow.SetActive(false);
        }
        else if (attackType == "Area")
        {
            area.SetActive(false);
        }
        else if (attackType == "Single_L")
        {
            lightningEnd.transform.position = lightningEndAtStart;
        }
        else if (attackType == "AreaSlow" || attackType == "AreaPoison")
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
    void AttackAnimation(GameObject enemy)
    {
        if (attackType == "Single")
        {
            arrow.transform.position = gameObject.transform.position + new Vector3(0,0.5f,0);
            arrow.GetComponent<Arrow>().SetSpeed((enemy.transform.position - gameObject.transform.position)/0.2f);
            arrow.SetActive(true);
            Invoke("A", 0.2f);
        }

        else if (attackType == "Area")
        {
            area.SetActive(true);
            Invoke("A", 1f); 
        }

        else if (attackType == "Single_L" || attackType == "Single_L2")
        {
            if (enemy){
                lightningEnd.transform.position = enemy.transform.position + new Vector3(0,0.5f,0); 
                Invoke("A", 0.1f);
            }
            else {
                lightningEnd.transform.position = lightningEndAtStart;
            }
            //Invoke("A", 0.1f);
        }

        else if (attackType == "AreaSlow" || attackType == "AreaPoison")
        {
            area.SetActive(true);
            if (attackType == "AreaSlow") Invoke("A", 1f); 
            else if(attackType == "AreaPoison") Invoke("A", 1f); 
        }
    }

    void Update()
    {
        
        timePassed += Time.deltaTime;
        if(timePassed > attackInterval)
        {
            if (attackType == "Single" || attackType == "Single_L")
            {
                GameObject nearestEnemy = GameData.getNearestObjectWithTag(transform.position, Tag);
                if(nearestEnemy && GameData.distanceRec(transform.position, nearestEnemy.transform.position) < attackRange)
                {
                    nearestEnemy.GetComponent<Enemy>().DeductHealth(Damage);
                    //line.SetPosition(1, nearestEnemy.transform.position + new Vector3(0,0.5f,0));
                    AttackAnimation(nearestEnemy);
                }
            }

            else if (attackType == "Single_L2")
            {
                GameObject[] enemies = GameData.getInRangeObjectWithTag(transform.position, Tag, attackRange);
                if (preAttackEnemy && ArrayUtility.Contains(enemies, preAttackEnemy))
                {
                    preAttackEnemy.GetComponent<Enemy>().DeductHealth(Damage);
                }
                else {
                    GameObject nearestEnemy = GameData.getNearestObjectWithTag(transform.position, Tag);
                    if(nearestEnemy && GameData.distanceRec(transform.position, nearestEnemy.transform.position) < attackRange)
                    {
                        nearestEnemy.GetComponent<Enemy>().DeductHealth(Damage);
                        preAttackEnemy = nearestEnemy;
                        //line.SetPosition(1, nearestEnemy.transform.position + new Vector3(0,0.5f,0));
                    }
                    else {
                        preAttackEnemy = null;
                    }
                }
                AttackAnimation(preAttackEnemy);
            }

            else if (attackType == "Area" || attackType == "AreaSlow" || attackType == "AreaPoison")
            {
                GameObject[] enemies = GameData.getInRangeObjectWithTag(transform.position, Tag, attackRange);
                if(enemies.Length > 0)
                {
                    foreach(GameObject enemy in enemies)
                    {
                        enemy.GetComponent<Enemy>().DeductHealth(Damage);
                        if (attackType == "AreaSlow" ) {
                            enemy.GetComponent<Enemy>().GiveEnemyDebuff(0, slowDownTime, "slow", slowDownRate);
                        } 
                        else if(attackType == "AreaPoison") {
                            enemy.GetComponent<Enemy>().GiveEnemyDebuff(1, slowDownTime, "poison", slowDownRate);
                        }
                    }
                    //area.SetActive(true);
                    //Invoke("A", 0.5f);
                    AttackAnimation(null);
                }
            }

            else if (attackType == "AreaRecover")
            {
                GameObject[] players = GameData.getInRangeObjectWithTag(transform.position, "Player", attackRange);
                if(players.Length > 0)
                {
                    foreach(GameObject player in players)
                    {
                        player.GetComponent<HPControl>().RecoverHP(Damage);
                    }
                    //area.SetActive(true);
                    //Invoke("A", 0.5f);
                    AttackAnimation(null);
                }
            }

            else if (attackType == "AreaBuff")
            {
                GameObject[] towers = GameData.getInRangeObjectWithTag(transform.position, "Tower", attackRange);
                if(towers.Length > 0)
                {
                    foreach(GameObject tower in towers)
                    {
                        tower.GetComponent<Tower>().BuffTower("damage", slowDownRate, slowDownTime);
                    }
                    //area.SetActive(true);
                    //Invoke("A", 0.5f);
                    AttackAnimation(null);
                }
            }
            timePassed = 0f;
        } 
    }
}
