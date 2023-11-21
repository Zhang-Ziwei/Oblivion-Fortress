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
    private Button upgradeButton1, upgradeButton2;
    // Start is called before the first frame update

    void Start()
    {
        upgradeButtonObject1 = transform.GetChild(1).gameObject;
        upgradeButtonObject2 = transform.GetChild(2).gameObject;

        upgradeButton1 = transform.GetChild(1).GetComponent<Button>();
        upgradeButton2 = transform.GetChild(2).GetComponent<Button>();
        deleteButton = transform.GetChild(3).GetComponent<Button>();
        
    }

    void UpdateTowerInfoUI(GameObject Tower){
        
        ResetAllButtons();

        //modify tower information
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Tower.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
        transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().Damage;
        transform.GetChild(0).GetChild(4).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackInterval;
        transform.GetChild(0).GetChild(5).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackRange;
        transform.GetChild(0).GetChild(6).GetChild(0).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackType;
        
        
        if (Tower.GetComponent<Tower>().upgradeBase1)
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
        }
        
        if (hitObject.transform.tag == "Tower")
        {
            upgradeButton1.interactable = true;
            upgradeButton2.interactable = true;
        }
        //Assign delete task to button
        deleteButton.onClick.AddListener(DeleteTaskOnClick);
        deleteButton.interactable = true;
    }

    void UpdateUpgradeTowerInfo(GameObject Base, GameObject upgradeButtonObject)
    {
        //Modify UI with tower information
        GameObject Tower = Base.GetComponent<Base>().tower;
        GameObject towerInfo = upgradeButtonObject.transform.GetChild(1).gameObject;
        towerInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "" + Base.GetComponent<Base>().MaxWood;
        towerInfo.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "" + Base.GetComponent<Base>().MaxStone;
        towerInfo.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().Damage;
        towerInfo.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackInterval;
        towerInfo.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackRange;
        towerInfo.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackType;

    }

    void UpgradeTaskOnClick1()
    {
        Instantiate(Tower.GetComponent<Tower>().upgradeBase1, hitObject.transform.position, Quaternion.identity);
        Destroy(hitObject);
        ResetAllButtons();
    }

    void UpgradeTaskOnClick2()
    {
        Instantiate(Tower.GetComponent<Tower>().upgradeBase2, hitObject.transform.position, Quaternion.identity);
        Destroy(hitObject);
        ResetAllButtons();
    }

    void DeleteTaskOnClick()
    {
        Destroy(hitObject);
        ResetAllButtons();
    }

    void ResetAllButtons()
    {
        upgradeButton1.onClick.RemoveAllListeners();
        upgradeButton2.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
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

            if(hit.transform.tag == "Base") Tower = hitObject.GetComponent<Base>().tower;
            else Tower = hitObject;

            UpdateTowerInfoUI(Tower);
        }              
    }
}
