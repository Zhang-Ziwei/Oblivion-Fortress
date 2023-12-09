using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class Slowness: PlayerDebuff
{
    public float ratio;

    public override void OnBuff(Enemy enemy) {
        buffName = "Slowness";
        base.OnBuff(enemy);
    }

    public override IEnumerator BuffCoroutine() {
        float originSpeed = playerController.movespeed;
        playerController.movespeed = originSpeed * ratio;
        yield return new WaitForSeconds(duration);
        playerController.movespeed = originSpeed;
        particle?.Stop();
        
        yield return new WaitForSeconds(cooldown);
        Destroy(nowItem);
    }
}