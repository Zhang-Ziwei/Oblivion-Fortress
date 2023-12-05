using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerUnlockManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int Exp = 0;
    public GameObject ExpObject;
    Dictionary<int, int> levelUnclocked;

    void Start()
    {
        levelUnclocked = new Dictionary<int, int>(){};
        ChangeUI();
    }

    public bool CheckUnclocked(int TowerID, int level){
        if (!levelUnclocked.ContainsKey(TowerID) || levelUnclocked[TowerID] < level) return false;
        else return true;
    }

    public bool SetUnclocked(int TowerID, int level, int unclockExp){
        if (Exp >= unclockExp) {
            Exp -= unclockExp;
            levelUnclocked[TowerID] = level;
            ChangeUI();
            return true;
        }
        return false;
    }

    public void GainExp(int exp){
        Exp += exp;
        ChangeUI();
    }


    public void ChangeUI(){
        ExpObject.GetComponent<Text>().text = "" + Exp;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
