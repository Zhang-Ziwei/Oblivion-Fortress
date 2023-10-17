using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ButtonDeleteTower : MonoBehaviour
{
    // Start is called before the first frame update
    private Button mybutton;
    private bool isDeleting = false;
    private GameObject preHitObject;

    void Start()
    {
        mybutton = GetComponent<Button>();
        mybutton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        isDeleting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDeleting)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if(preHitObject) preHitObject.GetComponent<Renderer>().material.color = Color.white;
            if(hit && (hit.transform.tag == "Base" || hit.transform.tag == "Tower"))
            {
                if(hit.transform.tag == "Base") preHitObject = hit.collider.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
                else if(hit.transform.tag == "Tower") preHitObject = hit.collider.gameObject.transform.GetChild(0).gameObject;
                preHitObject.GetComponent<Renderer>().material.color = Color.red;
                
            } 
            if(Input.GetMouseButtonDown(0))
            {
                if(hit && (hit.transform.tag == "Base" || hit.transform.tag == "Tower")) Destroy(hit.collider.gameObject);
                isDeleting = false;
                preHitObject = null;
            }              
        }
    }
}
