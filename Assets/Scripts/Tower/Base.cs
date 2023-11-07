using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Base : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tower;
    public int MaxWood = 3;
    public int Wood = 0;
    public int MaxStone = 3;
    public int Stone = 0;
    public bool DebugMode = false;
    private TMP_Text Text_Pro;

    void Start()
    {
        if(DebugMode) {
            Instantiate(tower, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }
        Text_Pro = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        changeUI();
    }

    private void changeUI()
    {
        Text_Pro.text = String.Format("W {0}/{1}\nS {2}/{3}", Wood, MaxWood, Stone, MaxStone);
    }

    public int depositWood(int depositNum) 
    {
        int remainNum = Math.Max(depositNum + Wood - MaxWood, 0);
        Wood += depositNum - remainNum;
        if ((Wood == MaxWood) && (Stone == MaxStone))
        {
            Instantiate(tower, transform.position, Quaternion.identity);
            Destroy(gameObject);
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
            Instantiate(tower, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        changeUI();
        return remainNum;
    }

}
