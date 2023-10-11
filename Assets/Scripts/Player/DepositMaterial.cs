using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositMaterial : MonoBehaviour
{
    //public string TagMask;
    //private HashSet<GameObject> _inrange = new HashSet<GameObject>();
    //private List<GameObject> list = new List<GameObject>();
    public float depositRange = 3;
    public string Tag = "Base";
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Debug.Log("e key was pressed");
            GameObject nearestBase = GameData.getNearestObjectWithTag(transform.position, Tag);
            if(nearestBase && GameData.distanceRec(transform.position, nearestBase.transform.position) < depositRange)
            {
                nearestBase.GetComponent<Base>().depositWood(1);
                nearestBase.GetComponent<Base>().depositStone(1);
            }
        }
        

        //Debug.Log(list);
    }
}
