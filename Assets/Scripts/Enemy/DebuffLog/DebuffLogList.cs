using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DebuffLogList : MonoBehaviour
{
    public DebuffLog debuffLog; // Prefab of your list item (Text, Image, etc.)

    private int numberOfItems = 0; // Initial number of items in the list

    private VerticalLayoutGroup verticalLayoutGroup;

    private Dictionary<string, Sprite> BuffIconRef;

    public static DebuffLogList Instance;

    void Start()
    {
        Instance = this;

        // Get the VerticalLayoutGroup component
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();

        Sprite[] BuffIconList = Resources.LoadAll<Sprite>("Enemies/DebuffIconData");

        BuffIconRef = new Dictionary<string, Sprite>();
        for (int i = 0; i < BuffIconList.Length; i++)
        {
            BuffIconRef.Add(BuffIconList[i].name, BuffIconList[i]);
            Debug.Log(BuffIconList[i].name);
        }

        numberOfItems = 0;

    }

    public void AddBuffItem(EnemyBuff enemyBuff)
    {
        numberOfItems++;

        // instantiate the new item prefab and set its parent to the content transform
        DebuffLog newItem = Instantiate(debuffLog, transform);

        

        newItem.Init(enemyBuff, BuffIconRef[enemyBuff.BuffName]);

    }

    public void RemoveBuffItem(DebuffLog itemToRemove)
    {
        numberOfItems--;

        // destroy the gameobject of the item to remove
        Destroy(itemToRemove.gameObject);
    }
}
