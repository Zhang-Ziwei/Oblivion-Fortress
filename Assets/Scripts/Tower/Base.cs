using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Base : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tower;
    public int MaxWood = 3;
    public int Wood = 0;
    public int MaxStone = 3;
    public int Stone = 0;
    public bool DebugMode = false;
    //private TMP_Text Text_Pro;

    void Start()
    {
        //Text_Pro = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        tower = transform.parent.GetChild(0).gameObject;
        changeUI();
    }

    private void changeUI()
    {   
        //Text_Pro.text = String.Format("W {0}/{1}\nS {2}/{3}", Wood, MaxWood, Stone, MaxStone);
        transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = Wood + "/" + MaxWood;
        transform.GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<Text>().text = Stone + "/" + MaxStone;
        
        if(DebugMode) {
            tower.SetActive(true); //Instantiate(tower, transform.position, Quaternion.identity);
            gameObject.SetActive(false);//Destroy(gameObject);
            return;
        }

        tower.GetComponent<Tower>().setUpgradeInfo();
    }

    public void setUpgradeResourse(int wood, int stone)
    {
        Wood = 0; Stone = 0;
        MaxWood = wood; MaxStone = stone;
        changeUI();
    }
    public int depositWood(int depositNum) 
    {
        int remainNum = Math.Max(depositNum + Wood - MaxWood, 0);
        Wood += depositNum - remainNum;
        if ((Wood == MaxWood) && (Stone == MaxStone))
        {
            tower.SetActive(true); //Instantiate(tower, transform.position, Quaternion.identity);
            gameObject.SetActive(false);//Destroy(gameObject);
        }
        changeUI();
        return remainNum;
    }

    public int depositStone(int depositNum) 
    {
        int remainNum = Math.Max(depositNum + Stone - MaxStone, 0);
        Stone += depositNum - remainNum;
        if ((Wood == MaxWood) && (Stone == MaxStone))
        {
            tower.SetActive(true); //Instantiate(tower, transform.position, Quaternion.identity);
            gameObject.SetActive(false);//Destroy(gameObject);
        }
        changeUI();
        return remainNum;
    }

}
