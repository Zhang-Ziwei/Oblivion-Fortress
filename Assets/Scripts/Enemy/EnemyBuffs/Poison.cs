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


    private new void Start() {
        base.Start();
        
        buffName = "Poison";

        if (damage <= 0) {
            Debug.Log("damage is not positive");
        }
        if (duration <= 0) {
            Debug.Log("duration is not positive");
        }
        if (interval <= 0) {
            Debug.Log("interval is not positive");
        }
    }
    public override IEnumerator BuffCoroutine() {
        isBuffed = true;
        float timer = 0;

        playerHP = player.GetComponent<HPControl>();
        while (timer < duration)
        {
            timer += interval;
            playerHP = player.GetComponent<HPControl>();
            playerHP.DeductHP(damage);
            yield return new WaitForSeconds(interval);
        }
        isBuffed = false;
    }
}