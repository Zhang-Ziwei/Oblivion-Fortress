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
        if (DebuffLogList.Instance.CheckDebuff(buffName)) {
            return;
        }
        base.OnBuff(enemy);
    }

    public override IEnumerator BuffCoroutine() {
        float timer = 0;

        playerHP = player.GetComponent<HPControl>();
        while (timer < duration)
        {
            timer += interval;
            playerHP = player.GetComponent<HPControl>();
            if(playerHP.HP > 0){
                playerHP.DeductHP(damage);
            }
            yield return new WaitForSeconds(interval);
            
        }
        if (nowItem != null)
        {
            particle?.Stop();
        }
        yield return new WaitForSeconds(cooldown);
        Destroy(nowItem);
    }
}