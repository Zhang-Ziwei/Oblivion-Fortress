using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    private TMP_Text Text_Pro;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Text_Pro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Text_Pro.text = String.Format("Wood: {0}\n(Limit: 2)", player.GetComponent<CollectResource>().wood);
    }
}
