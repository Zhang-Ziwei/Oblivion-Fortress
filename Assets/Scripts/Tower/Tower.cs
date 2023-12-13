using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class LevelUp
{
    public int unlockExp;
    public int maxWood1;
    public int maxStone1;
    public string buffTarget1;
    public float buffValue1;
    public string buffInfo1;
    public int maxWood2;
    public int maxStone2;
    public string buffTarget2;
    public float buffValue2;
    public string buffInfo2;
};
public class TowerBuff
{
    public string id;
    public string buffTarget;
    public float buffValue;
    public TowerBuff(string oid, string target, float value){
        id = oid;
        buffTarget = target;
        buffValue = value;
    }
};
public class Tower : MonoBehaviour
{
    
    public float attackRange = 5;
    public float Damage = 1F;
    public float maxDamageRate = 1F; // Use for tower 5
    public float DamageIncreaseRate = 1.1F; // Use for tower 5
    public float slowDownValue = 0.5f; //Use for slow tower
    public float slowDownTime = 5;  //Use for slow tower
    public string Tag = "Enemy";
    public float attackInterval = 1f;
    public string attackType = "Single";
    public int ID = 0;
    public LevelUp[] levelUps; 
    public int level = 1;
    private float timePassed = 0f;
    private float DamageRate = 1f;

    // used for attack animation
    private LineRenderer line;
    private GameObject area;
    private GameObject lightningEnd;
    private GameObject lightning2;
    private GameObject lightning3;
    private Vector3 lightningEndAtStart;
    private GameObject arrow; 

    private GameObject preAttackEnemy = null;

    public GameObject upgradeTB1;
    public GameObject upgradeTB2;

    private GameObject Base;
    private List<TowerBuff> towerBuffs = new List<TowerBuff>();
    private List<float> BuffTimers = new List<float>(); 
    private bool SpecialUpgrade = false;
    private List<GameObject> enemies; 
    public float percentHPDamage = 0f;
    public bool isUpgradeUnlocked = false;

    public void setUpgradeInfo(){
        for (int i = 0; i < levelUps.Length; i++){
            string buffTarget = levelUps[i].buffTarget1; 
            if (buffTarget == "Damage") {
                //Damage += buffValue;
                levelUps[i].buffInfo1 = "Damage  +" + levelUps[i].buffValue1;
            }
            else if (buffTarget == "Interval") {
                //attackInterval *= buffValue;
                levelUps[i].buffInfo1 = "Attack Interval  x" + levelUps[i].buffValue1;
            }
            else if (buffTarget == "Range") {
                //attackRange += buffValue;
                levelUps[i].buffInfo1 = "Attack Range  +" + levelUps[i].buffValue1;
            }
            else if (buffTarget == "SlowDownTime") {
                //slowDownTime += buffValue;
                levelUps[i].buffInfo1 = "Slow Down Time  +" + levelUps[i].buffValue1;
            }
            else if (buffTarget == "PercentHP") {
                //slowDownTime += buffValue;
                levelUps[i].buffInfo1 = "Attack +" + levelUps[i].buffValue1 * 100f + "% current HP damage";
            }
            /*else if (buffTarget == "Special") {
                //SpecialUpgrade = true;
            }
            else if (buffTarget == "Tranform") {
                Instantiate(upgradeTB1, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }*/

            buffTarget = levelUps[i].buffTarget2; 
            if (buffTarget == "Damage") {
                //Damage += buffValue;
                levelUps[i].buffInfo2 = "Damage  +" + levelUps[i].buffValue2;
            }
            else if (buffTarget == "Interval") {
                //attackInterval *= buffValue;
                levelUps[i].buffInfo2 = "Attack Interval  x" + levelUps[i].buffValue2;
            }
            else if (buffTarget == "Range") {
                //attackRange += buffValue;
                levelUps[i].buffInfo2 = "Attack Range  +" + levelUps[i].buffValue2;
            }
            else if (buffTarget == "SlowDownTime") {
                //slowDownTime += buffValue;
                levelUps[i].buffInfo2 = "Slow Down Time  +" + levelUps[i].buffValue2;
            }
            /*else if (buffTarget == "Special") {
                //SpecialUpgrade = true;
            }
            else if (buffTarget == "Tranform") {
                Instantiate(upgradeTB1, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }*/
        }
    }

    public LevelUp getLevelUpInfo(){
        if (level > levelUps.Length) return null; 
        return levelUps[level-1];
    }

    public void Upgrade(int maxWood, int maxStone, string buffTarget, float buffValue, int choice = 1){
        if (buffTarget == "Damage") {
            Damage += buffValue;
        }
        else if (buffTarget == "Interval") {
            attackInterval *= buffValue;
        }
        else if (buffTarget == "Range") {
            attackRange += buffValue;
        }
        else if (buffTarget == "SlowDownTime") {
            slowDownTime += buffValue;
        }
        else if (buffTarget == "PercentHP") {
            percentHPDamage += buffValue;
        }
        else if (buffTarget == "Special") {
            SpecialUpgrade = true;
        }
        else if (buffTarget == "Transform") {
            if (choice == 1) Instantiate(upgradeTB1, transform.position, Quaternion.identity);
            else Instantiate(upgradeTB2, transform.position, Quaternion.identity);
            Destroy(gameObject.transform.parent.gameObject);
        }

        level++;
        isUpgradeUnlocked = false;
        gameObject.SetActive(false);
        Base.SetActive(true);
        Base.GetComponent<Base>().setUpgradeResourse(maxWood, maxStone);
    }


    public void towerUpgrade(int id){
        if (id == 1) Upgrade(levelUps[level-1].maxWood1, levelUps[level-1].maxStone1, levelUps[level-1].buffTarget1, levelUps[level-1].buffValue1);
        else Upgrade(levelUps[level-1].maxWood2, levelUps[level-1].maxStone2, levelUps[level-1].buffTarget2, levelUps[level-1].buffValue2, 2);
    }

    public void BuffTower(string id, string buffName, float buffValue, float buffTime){
        if (attackType == "AreaBuff") return;

        bool idExist = false;

        for(int i = towerBuffs.Count - 1; i >= 0; i--){
            if (towerBuffs[i].id == id){
                if (BuffTimers[i] < buffTime){
                    BuffTimers[i] = buffTime;
                    idExist = true;
                    break;
                }
            }
        }

        if(!idExist){
            towerBuffs.Add(new TowerBuff(id, buffName, buffValue));
            BuffTimers.Add(buffTime);
            if (buffName == "Damage"){
                Damage += buffValue;
            //StartCoroutine(RestoreBuff(buffName, buffValue, buffTime));
            }
            else if (buffName == "Interval"){
                attackInterval *= buffValue;
                //StartCoroutine(RestoreBuff(buffName, buffValue, buffTime));
            }
            else if (buffName == "PercentHP"){
                percentHPDamage += buffValue;
            }
        }
    }

    private void UpdateBuff(){
        for(int i = towerBuffs.Count - 1; i >= 0; i--){
            BuffTimers[i] -= Time.deltaTime;
            if (BuffTimers[i] <= 0) {
                if (towerBuffs[i].buffTarget == "Damage"){
                    Damage -= towerBuffs[i].buffValue;
                }
                else if (towerBuffs[i].buffTarget == "Interval"){
                    attackInterval /= towerBuffs[i].buffValue;
                }
                else if (towerBuffs[i].buffTarget == "PercentHP"){
                    percentHPDamage -= towerBuffs[i].buffValue;
                }
                towerBuffs.RemoveAt(i);
                BuffTimers.RemoveAt(i);
            }
        }
    }

    void Start()
    {
        enemies = new List<GameObject>();
        Base = transform.parent.GetChild(1).gameObject;
        setUpgradeInfo();
        if (attackType == "Single")
        {
            //line = gameObject.GetComponent<LineRenderer>();
            //line.positionCount = 2;
            //line.SetPosition(0, transform.position + new Vector3(0,0.5f,0));
            //line.SetPosition(1, transform.position + new Vector3(0,0.5f,0));
            arrow = gameObject.transform.GetChild(2).gameObject; 
        }
        else if (attackType == "Area" || attackType == "AreaSlow" || attackType == "AreaPoison" || attackType == "AreaRecover" || attackType == "AreaBuff" || attackType == "AreaMaxHP")
        {
            area = gameObject.transform.GetChild(2).gameObject;
            //area.transform.localScale = new Vector3(attackRange, attackRange, 1);
        }
        else if (attackType == "Single_L" || attackType == "Single_L2")
        {
            lightningEnd = gameObject.transform.GetChild(2).GetChild(1).gameObject;
            lightningEndAtStart = lightningEnd.transform.position;
            if (attackType == "Single_L") {
                lightning2 = transform.GetChild(3).gameObject;
                lightning3 = transform.GetChild(4).gameObject;
            }
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
            lightning2.SetActive(false);
            lightning3.SetActive(false);
        }
        else if (attackType == "AreaSlow" || attackType == "AreaPoison" || attackType == "AreaMaxHP")
        {
            area.SetActive(false);
        }
        else if (attackType == "Single_L2")
        {
            if (preAttackEnemy){
                lightningEnd.transform.position = preAttackEnemy.transform.position + new Vector3(0,0.5f,0); 
                Invoke("A", 0.1f);
            }
            else {
                lightningEnd.transform.position = lightningEndAtStart;
            }
            //Invoke("A", 0.1f);
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

        else if (attackType == "AreaSlow" || attackType == "AreaPoison" || attackType == "AreaMaxHP")
        {
            area.SetActive(true);
            Invoke("A", 1f); 
        }
    }

    void AttackAnimation2(List<GameObject> enemies)
    {
        if (attackType == "Single_L" || attackType == "Single_L2")
        {
            if (enemies.Count > 0){
                lightningEnd.transform.position = enemies[0].transform.position + new Vector3(0,0.5f,0); 
                if (enemies.Count > 1) {
                    lightning2.SetActive(true);
                    lightning2.transform.GetChild(0).position = lightningEnd.transform.position;
                    lightning2.transform.GetChild(1).position = enemies[1].transform.position + new Vector3(0,0.5f,0); 
                    if (enemies.Count > 2) {
                        lightning3.SetActive(true);
                        lightning3.transform.GetChild(0).position = enemies[1].transform.position + new Vector3(0,0.5f,0); 
                        lightning3.transform.GetChild(1).position = enemies[2].transform.position + new Vector3(0,0.5f,0); 
                    }
                }
                Invoke("A", 0.1f);
            }
            else {
                lightningEnd.transform.position = lightningEndAtStart;
            }
            //Invoke("A", 0.1f);
        }
    }

    void Update()
    {
        UpdateBuff();
        timePassed += Time.deltaTime;
        if(timePassed > attackInterval)
        {
            if (attackType == "Single" || attackType == "Single_L")
            {
                enemies.Clear();
                GameObject nearestEnemy = GameData.getNearestObjectWithTag(transform.position, Tag), nearestEnemy2, nearestEnemy3;
                if(nearestEnemy && GameData.distanceRec(transform.position, nearestEnemy.transform.position) < attackRange)
                {
                    enemies.Add(nearestEnemy);
                    if (SpecialUpgrade) {
                        nearestEnemy2 = GameData.getNearestObjectWithTag(nearestEnemy.transform.position, Tag);
                        if(nearestEnemy2 && GameData.distanceRec(nearestEnemy.transform.position, nearestEnemy2.transform.position) < 3)
                        {
                            enemies.Add(nearestEnemy2);
                            nearestEnemy3 = GameData.getNearestObjectWithTag(nearestEnemy2.transform.position, Tag);
                            //if (nearestEnemy3 == nearestEnemy) nearestEnemy3 = GameData.getNearestObjectWithTag(nearestEnemy2.transform.position, Tag, GameData.distanceRec(nearestEnemy.transform.position, nearestEnemy2.transform.position));
                            if (nearestEnemy3 && GameData.distanceRec(nearestEnemy2.transform.position, nearestEnemy3.transform.position) < 3)
                            {
                                enemies.Add(nearestEnemy3);
                            }
                        }
                        else {
                            enemies.Add(nearestEnemy);
                            enemies.Add(nearestEnemy);
                        }
                    }
                    for (int i= 0; i < enemies.Count; i++) {
                        if(i == 0) enemies[i].GetComponent<Enemy>().DeductHealth(Damage, percentHPDamage);
                        else enemies[i].GetComponent<Enemy>().DeductHealth(Damage/2, percentHPDamage);
                    }
                    //line.SetPosition(1, nearestEnemy.transform.position + new Vector3(0,0.5f,0));
                    
                    AttackAnimation2(enemies);
                }
            }

            else if (attackType == "Single_L2")
            {
                GameObject[] enemies = GameData.getInRangeObjectWithTag(transform.position, Tag, attackRange);
                if (preAttackEnemy && Array.FindAll(enemies ,e => e == preAttackEnemy).Length > 0)
                {
                    DamageRate = Math.Min(DamageIncreaseRate * DamageRate, maxDamageRate);
                    preAttackEnemy.GetComponent<Enemy>().DeductHealth(Damage * DamageRate, percentHPDamage);
                }
                else {
                    GameObject nearestEnemy = GameData.getNearestObjectWithTag(transform.position, Tag);
                    if(nearestEnemy && GameData.distanceRec(transform.position, nearestEnemy.transform.position) < attackRange)
                    {
                        DamageRate = 1;
                        nearestEnemy.GetComponent<Enemy>().DeductHealth(Damage * DamageRate, percentHPDamage);
                        preAttackEnemy = nearestEnemy;
                        //line.SetPosition(1, nearestEnemy.transform.position + new Vector3(0,0.5f,0));
                    }
                    else {
                        preAttackEnemy = null;
                    }
                }
                AttackAnimation(preAttackEnemy);
            }

            else if (attackType == "Area" || attackType == "AreaSlow" || attackType == "AreaPoison" || attackType == "AreaMaxHP")
            {
                GameObject[] enemies = GameData.getInRangeObjectWithTag(transform.position, Tag, attackRange);
                if(enemies.Length > 0)
                {
                    foreach(GameObject enemy in enemies)
                    {
                        enemy.GetComponent<Enemy>().DeductHealth(Damage, percentHPDamage);
                        /*if (attackType == "Area" && SpecialUpgrade) {
                            enemy.GetComponent<Enemy>().DeductHealthPercent(Damage * 0.005f);
                        }*/
                        if (attackType == "AreaSlow" ) {
                            if (slowDownValue < 0.1) enemy.GetComponent<Enemy>().GiveEnemyDebuff(3, slowDownTime, "slow", slowDownValue);
                            else enemy.GetComponent<Enemy>().GiveEnemyDebuff(0, slowDownTime, "slow", slowDownValue);
                        } 
                        else if(attackType == "AreaPoison") {
                            enemy.GetComponent<Enemy>().GiveEnemyDebuff(1, slowDownTime, "poison", Damage);
                        }
                        else if(attackType == "AreaMaxHP") {
                            enemy.GetComponent<Enemy>().GiveEnemyDebuff(2, slowDownTime, "reduceMaxHP", slowDownValue);
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
                        if(!player.GetComponent<HPControl>().isPlayerDie()) player.GetComponent<HPControl>().RecoverHP(Damage);
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
                        tower.GetComponent<Tower>().BuffTower(GetInstanceID() + "D" + level, "Damage", Damage, 1);
                        tower.GetComponent<Tower>().BuffTower(GetInstanceID() + "I" + level, "Interval", attackInterval, 1);
                        tower.GetComponent<Tower>().BuffTower(GetInstanceID() + "P" + level, "PercentHP", percentHPDamage, 1);
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
