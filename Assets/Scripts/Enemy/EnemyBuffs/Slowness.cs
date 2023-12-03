using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class Slowness: EnemyBuff
{
    public float ratio;

    public override void Buff()
    {
        if (DebuffLogList.Instance.CheckDebuff(buffName)) {
            return;
        }

        player = GameObject.Find("Player");
        playerController = player.GetComponent<HeroKnight>();
        nowItem = Instantiate(gameObject, player.transform.position, Quaternion.identity);

        buffName = "Slowness";

        DebuffLogList.Instance.AddBuffItem(this);

        // set the parent of the gameObject to player
        nowItem.transform.SetParent(player.transform);

        player.GetComponent<MonoBehaviour>().StartCoroutine(BuffCoroutine());
    }

    public override IEnumerator BuffCoroutine() {
        float originSpeed = playerController.movespeed;
        playerController.movespeed = originSpeed * ratio;
        yield return new WaitForSeconds(duration);
        playerController.movespeed = originSpeed;
        Destroy(nowItem);
        yield return new WaitForSeconds(cooldown);
    }
}