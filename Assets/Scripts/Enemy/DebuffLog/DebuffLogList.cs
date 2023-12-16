using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DebuffLogList : MonoBehaviour
{
    public DebuffLog debuffLog; // Prefab of your list item (Text, Image, etc.)

    private int numberOfItems = 0; // Initial number of items in the list

    private VerticalLayoutGroup verticalLayoutGroup;

    private Dictionary<string, int> nowDebuffs;
    public static DebuffLogList Instance;

    void Start()
    {
        Instance = this;

        // Get the VerticalLayoutGroup component
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();

        numberOfItems = 0;
        
        nowDebuffs = new Dictionary<string, int>();

    }
    void Update()
    {

    }
    public bool CheckDebuff(string debuffName)
    {
        if (!nowDebuffs.ContainsKey(debuffName))
        {
            nowDebuffs.Add(debuffName, 0);
        }
        if (nowDebuffs[debuffName] == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddBuffItem(Buff buff)
    {
        numberOfItems++;
        nowDebuffs[buff.BuffName]++;

        // instantiate the new item prefab and set its parent to the content transform
        DebuffLog newItem = Instantiate(debuffLog, transform);

        newItem.Init(buff);

    }

    public void RemoveBuffItem(DebuffLog itemToRemove)
    {
        StartCoroutine(RemoveBuffItemCoroutine(itemToRemove));
    }
    private IEnumerator RemoveBuffItemCoroutine(DebuffLog itemToRemove)
    {
        // destroy the gameobject of the item to remove
        Destroy(itemToRemove.gameObject);
        yield return new WaitForSeconds(itemToRemove.myBuff.cooldown);
        numberOfItems--;
        nowDebuffs[itemToRemove.myBuff.BuffName]--;
    }
}
