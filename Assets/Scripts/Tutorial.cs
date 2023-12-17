using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int stage = 1;

    public GameObject help1;
    public GameObject help2;
    public GameObject help3;
    public GameObject help4;
    public GameObject help5;
    public GameObject help6;
    public GameObject help7;
    public GameObject help8;
    public GameObject help9;
    public GameObject help10;
    public GameObject help11;
    public GameObject help12;
    public GameObject help13;

    public GameObject towerinfo;

    public GameObject prohibit;
    public GameObject base_prohibit;
    public GameObject base_locate;
    public GameObject base_frame;
    public GameObject LevelManager;
    public GameObject BackgroundMusic;
    
    PickupSystem pickup;
    HPControl playerHP;
    Castle castle;
    GameObject towerbase;
    GameObject tree;
    GameObject tower;
    GameObject enemy;
    bool hadspawned = false;

    // Start is called before the first frame update
    void Start()
    {
        help1.SetActive(true);
        Time.timeScale = 0;
        pickup = GameObject.Find("Player").GetComponent<PickupSystem>();
        playerHP = GameObject.Find("Player").GetComponent<HPControl>();
        castle = GameObject.Find("castle").GetComponent<Castle>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stage == 7)
        {
            towerbase = GameObject.Find("TB 1(Clone)");
            if(towerbase != null)
            {
                help7.SetActive(false);
                base_prohibit.SetActive(false);
                base_locate.SetActive(false);
                base_frame.SetActive(false);
                help8.SetActive(true);
                stage = 8;
            }
        }
        if(stage == 8)
        {
            if(pickup.type == 1) // if holding axe
            {
                help8.SetActive(false);
                help9.SetActive(true);
                stage = 9;
            }
        }
        if(stage == 9)
        {
            tree = GameObject.Find("Tree");
            if(tree == null)
            {
                help9.SetActive(false);
                help10.SetActive(true);
                stage = 10;
            }
        }
        if(stage == 10)
        {
            tower = GameObject.Find("Tower 1");
            if (tower != null)
            {
                help10.SetActive(false);
                help11.SetActive(true);
                stage = 11;
                Invoke("EnemyStart", 5);
            }
        }
        if(stage == 12)
        {
            if(playerHP.HP < playerHP.maxHP || castle.health < castle.maxHealth)
            {
                help12.SetActive(true);
                Invoke("closehelp12", 10);
            }
            enemy = GameObject.Find("Skeleton(Clone)");
            if(enemy != null)
            {
                hadspawned = true;
            }
            if(enemy == null && hadspawned)
            {
                stage = 13;
                closehelp12();
                help13.SetActive(true);
                Invoke("closehelp13", 10);
            }
        }
    }

    public void bottom1to2()
    {
        help1.SetActive(false);
        help2.SetActive(true);
    }
    public void bottom2to3()
    {
        help2.SetActive(false);
        help3.SetActive(true);
    }
    public void bottom3to4()
    {
        help3.SetActive(false);
        help4.SetActive(true);
        towerinfo.SetActive(true);
    }
    public void bottom4to5()
    {
        help4.SetActive(false);
        towerinfo.SetActive(false);
        help5.SetActive(true);
    }
    public void bottom5to6()
    {
        help5.SetActive(false);
        help6.SetActive(true);
        Time.timeScale = 1;
    }
    public void bottom6to7()
    {
        help6.SetActive(false);
        prohibit.SetActive(false);
        help7.SetActive(true);
        base_prohibit.SetActive(true);
        base_locate.SetActive(true);
        base_frame.SetActive(true);
        stage = 7;
    }

    void EnemyStart()
    {
        BackgroundMusic.SetActive(false);
        LevelManager.SetActive(true);
        help11.SetActive(false);
        stage = 12;
    }
    void closehelp12()
    {
        help12.SetActive(false);
    }
    void closehelp13()
    {
        help13.SetActive(false);
    }
}
