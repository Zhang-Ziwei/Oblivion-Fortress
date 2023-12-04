using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ButtonSetBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    private Button mybutton;
    public GameObject Square;
    public GameObject TB;
    private GameObject Base;
    public GameObject attackField;
    private Collider2D BaseCollider;
    private bool isBaseSetting = false;
    private float attackRange;
    private GameObject towerInfo;
    private GameObject Tower;

    void Start()
    {
        mybutton = GetComponent<Button>();
        mybutton.onClick.AddListener(TaskOnClick);

        //Set base UI
        Square = Instantiate(Square);
        Square.SetActive(false);
        BaseCollider = Square.GetComponent<PolygonCollider2D>();

        //Get correspond tower of base
        Tower = TB.transform.GetChild(0).gameObject; //Tower = Base.GetComponent<Base>().tower;
        Base = TB.transform.GetChild(1).gameObject;
        attackRange = Tower.GetComponent<Tower>().attackRange;
        attackField = Instantiate(attackField);
        attackField.transform.localScale = new Vector3(attackRange, attackRange, 1);
        attackField.transform.parent = Square.transform;

        //Modify UI with tower information
        towerInfo = transform.GetChild(1).gameObject;
        towerInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "" + Base.GetComponent<Base>().MaxWood;
        towerInfo.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "" + Base.GetComponent<Base>().MaxStone;
        towerInfo.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().Damage;
        towerInfo.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackInterval;
        towerInfo.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackRange;
        towerInfo.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "" + Tower.GetComponent<Tower>().attackType;
    }

    void TaskOnClick()
    {
        isBaseSetting = true;
        Square.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData){
        towerInfo.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData){
        towerInfo.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCollider(BaseCollider, new ContactFilter2D().NoFilter(), new List<Collider2D>()) > 0)
        {
            Square.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            Square.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        if(isBaseSetting && Input.GetMouseButtonDown(0))
        {
            // Check if the collider of new base overlapping with other colliders 
            
            if(Physics2D.OverlapCollider(BaseCollider, new ContactFilter2D().NoFilter(), new List<Collider2D>()) == 0)
            {
                Instantiate(TB, GameData.nearestVertex(GameData.getMousePos()), Quaternion.identity);
            }
            isBaseSetting = false;
            Square.SetActive(false);
        }
    }
}
