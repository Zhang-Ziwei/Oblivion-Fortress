using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ButtonSetBase : MonoBehaviour
{
    // Start is called before the first frame update
    private Button mybutton;
    public GameObject Square;
    public GameObject Base;
    public GameObject attackField;
    private Collider2D BaseCollider;
    private bool isBaseSetting = false;
    private float attackRange;

    void Start()
    {
        mybutton = GetComponent<Button>();
        mybutton.onClick.AddListener(TaskOnClick);
        Square = Instantiate(Square);
        Square.SetActive(false);
        BaseCollider = Square.GetComponent<PolygonCollider2D>();
        attackRange = Base.GetComponent<Base>().tower.GetComponent<TowerAttack>().attackRange;
        attackField = Instantiate(attackField);
        attackField.transform.localScale = new Vector3(attackRange, attackRange, 1);
        attackField.transform.parent = Square.transform;
    }

    void TaskOnClick()
    {
        isBaseSetting = true;
        Square.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if(isBaseSetting && Input.GetMouseButtonDown(0))
        {
            // Check if the collider of new base overlapping with other colliders 
            if(Physics2D.OverlapCollider(BaseCollider, new ContactFilter2D().NoFilter(), new List<Collider2D>()) == 0)
            {
                Instantiate(Base, GameData.nearestVertex(GameData.getMousePos()), Quaternion.identity);
                isBaseSetting = false;
                Square.SetActive(false);
            }
        }
    }
}
