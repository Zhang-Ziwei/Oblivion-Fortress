using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour
{
    private GameObject hitObject;
    private GameObject Tower;
    private Button deleteButton;
    private GameObject upgradeButtonObject1, upgradeButtonObject2;
    private GameObject unlockButtonObject;
    private Button upgradeButton1, upgradeButton2;
    private Button unlockButton;
    private GameObject Manager;
    public LevelUp towerLevelUp;
    // Start is called before the first frame update

    void Start()
    {
        upgradeButtonObject1 = transform.GetChild(1).gameObject;
        upgradeButtonObject2 = transform.GetChild(2).gameObject;
        unlockButtonObject = transform.GetChild(4).gameObject;

        upgradeButton1 = transform.GetChild(1).GetComponent<Button>();
        upgradeButton2 = transform.GetChild(2).GetComponent<Button>();
        deleteButton = transform.GetChild(3).GetComponent<Button>();
        unlockButton = transform.GetChild(4).GetComponent<Button>();

        Manager = GameObject.FindGameObjectsWithTag("Manager")[0];
    }

    void UpdateTowerInfoUI(GameObject Tower){
        
        ResetAllButtons();

        //modify tower information
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Tower.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
        transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().Damage;
        transform.GetChild(0).GetChild(4).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackInterval;
        transform.GetChild(0).GetChild(5).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackRange;
        transform.GetChild(0).GetChild(6).GetChild(0).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackType;
        
        towerLevelUp = Tower.GetComponent<Tower>().getLevelUpInfo();

        /*if (Tower.GetComponent<Tower>().upgradeBase1)
        {
            upgradeButtonObject1.SetActive(true);
            UpdateUpgradeTowerInfo(Tower.GetComponent<Tower>().upgradeBase1, upgradeButtonObject1);
            upgradeButton1.onClick.AddListener(UpgradeTaskOnClick1);
        }else {
            upgradeButtonObject1.SetActive(false);
        }

        if (Tower.GetComponent<Tower>().upgradeBase2)
        {
            upgradeButtonObject2.SetActive(true);
            UpdateUpgradeTowerInfo(Tower.GetComponent<Tower>().upgradeBase2, upgradeButtonObject2);
            upgradeButton2.onClick.AddListener(UpgradeTaskOnClick2);
        }else {
            upgradeButtonObject2.SetActive(false);
        }*/

        if (towerLevelUp == null) 
        {
            upgradeButtonObject1.SetActive(false);
            upgradeButtonObject2.SetActive(false);
            unlockButtonObject.SetActive(false);
        }
        else {
            UpdateUpgradeInfo();
            upgradeButtonObject1.SetActive(true);
            upgradeButtonObject2.SetActive(true);
            if (towerLevelUp.unlockExp == 0 || Manager.GetComponent<TowerUnlockManager>().CheckUnclocked(Tower.GetComponent<Tower>().ID, Tower.GetComponent<Tower>().level)) 
            {
                unlockButtonObject.SetActive(false);

                if (hitObject.transform.tag == "Tower") 
                {
                    upgradeButton1.onClick.AddListener(UpgradeTaskOnClick1);
                    upgradeButton2.onClick.AddListener(UpgradeTaskOnClick2);
                    upgradeButton1.interactable = true;
                    upgradeButton2.interactable = true;
                }
                else {
                    upgradeButton1.interactable = false;
                    upgradeButton2.interactable = false;
                }
            }
            else {
                unlockButtonObject.SetActive(true);
                unlockButton.onClick.AddListener(UnlockTaskOnClick);
                upgradeButton1.interactable = false;
                upgradeButton2.interactable = false;
            }
        }

        //Assign delete task to button
        deleteButton.onClick.AddListener(DeleteTaskOnClick);
        deleteButton.interactable = true;
    }

    void UpdateUpgradeInfo()
    {
        //Modify UI with tower information
        upgradeButtonObject1.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text = "" + towerLevelUp.maxWood1;
        upgradeButtonObject1.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Text>().text = "" + towerLevelUp.maxStone1;
        upgradeButtonObject1.transform.GetChild(1).GetChild(5).GetChild(0).GetComponent<Text>().text = "" + towerLevelUp.buffInfo1;

        upgradeButtonObject2.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text = "" + towerLevelUp.maxWood2;
        upgradeButtonObject2.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Text>().text = "" + towerLevelUp.maxStone2;
        upgradeButtonObject2.transform.GetChild(1).GetChild(5).GetChild(0).GetComponent<Text>().text = "" + towerLevelUp.buffInfo2;

        unlockButtonObject.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "" + towerLevelUp.unlockExp;
    }
    /*void UpdateUpgradeTowerInfo(GameObject TB, GameObject upgradeButtonObject)
    {
        //Modify UI with tower information
        GameObject Tower = TB.transform.GetChild(0).gameObject;
        GameObject Base = TB.transform.GetChild(1).gameObject;
        GameObject towerInfo = upgradeButtonObject.transform.GetChild(1).gameObject;
        towerInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "" + Base.GetComponent<Base>().MaxWood;
        towerInfo.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "" + Base.GetComponent<Base>().MaxStone;
        towerInfo.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().Damage;
        towerInfo.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackInterval;
        towerInfo.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackRange;
        towerInfo.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackType;

    }*/

    void UpgradeTaskOnClick1()
    {
        Tower.GetComponent<Tower>().towerUpgrade(1);
        ResetAllButtons();
        UpdateTowerInfoUI(Tower);
    }

    void UpgradeTaskOnClick2()
    {
        Tower.GetComponent<Tower>().towerUpgrade(2);
        ResetAllButtons();
        UpdateTowerInfoUI(Tower);
    }

    void DeleteTaskOnClick()
    {
        Destroy(hitObject.transform.parent.gameObject);
        ResetAllButtons();
    }

    void UnlockTaskOnClick()
    {
        Manager.GetComponent<TowerUnlockManager>().SetUnclocked(Tower.GetComponent<Tower>().ID, Tower.GetComponent<Tower>().level, towerLevelUp.unlockExp); 
        ResetAllButtons();
        UpdateTowerInfoUI(Tower);
    }

    void ResetAllButtons()
    {
        upgradeButton1.onClick.RemoveAllListeners();
        upgradeButton2.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
        unlockButton.onClick.RemoveAllListeners();
        upgradeButton1.interactable = false;
        upgradeButton2.interactable = false;
        deleteButton.interactable = false;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);   
        //if(preHitObject) preHitObject.GetComponent<Renderer>().material.color = Color.white;
        if(Input.GetMouseButtonDown(0) && hit && (hit.transform.tag == "Base" || hit.transform.tag == "Tower"))
        {
            hitObject = hit.collider.gameObject;

            if(hit.transform.tag == "Base") Tower = hitObject.transform.parent.GetChild(0).gameObject;
            else Tower = hitObject;

            UpdateTowerInfoUI(Tower);
        }              
    }
}
