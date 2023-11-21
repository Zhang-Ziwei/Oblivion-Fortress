using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    private GameObject towerInfo;
    void Start()
    {
        towerInfo = transform.GetChild(1).gameObject;
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

    }
}
