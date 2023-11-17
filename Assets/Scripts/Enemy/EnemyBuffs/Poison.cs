using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class Poison: EnemyBuff
{
    public float damage;

    public float duration;

    public float interval;
    public override IEnumerator BuffCoroutine() {
        isBuffed = true;
        float timer = 0;
        HPControl playerHP = player.GetComponent<HPControl>();
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