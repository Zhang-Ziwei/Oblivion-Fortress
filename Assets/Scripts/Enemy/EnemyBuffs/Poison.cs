using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;
using Unity.VisualScripting;

public class Poison: PlayerDebuff
{
    public float damage;

    public float interval;

    private HPControl playerHP;

    public override void OnBuff(Enemy enemy) {
        buffName = "Poison";
        base.OnBuff(enemy);
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
        if (particle != null && this != null)
            particle.Stop();
        yield return new WaitForSeconds(cooldown);
        Destroy(nowItem);
    }
}