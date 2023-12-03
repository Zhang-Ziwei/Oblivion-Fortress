using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;
using Unity.VisualScripting;

public class Poison: EnemyBuff
{
    public float damage;

    public float interval;

    private HPControl playerHP;

    public override void Buff()
    {
        if (DebuffLogList.Instance.CheckDebuff(buffName)) {
            return;
        }

        player = GameObject.Find("Player");
        playerController = player.GetComponent<HeroKnight>();
        nowItem = Instantiate(gameObject, player.transform.position, Quaternion.identity);

        buffName = "Poison";

        DebuffLogList.Instance.AddBuffItem(this);

        // set the parent of the gameObject to player
        nowItem.transform.SetParent(player.transform);

        player.GetComponent<MonoBehaviour>().StartCoroutine(BuffCoroutine());
    }
    public override IEnumerator BuffCoroutine() {
        float timer = 0;

        playerHP = player.GetComponent<HPControl>();
        while (timer < duration)
        {
            timer += interval;
            playerHP = player.GetComponent<HPControl>();
            playerHP.DeductHP(damage);
            yield return new WaitForSeconds(interval);
            
        }
        Destroy(nowItem);
        yield return new WaitForSeconds(cooldown);
    }
}