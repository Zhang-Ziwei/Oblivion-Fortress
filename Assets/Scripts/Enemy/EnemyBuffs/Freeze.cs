using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class Freeze: PlayerDebuff
{
    public override void OnBuff(Enemy enemy) {
        buffName = "Freeze";
        base.OnBuff(enemy);
    }
    public override IEnumerator BuffCoroutine() {
        float originSpeed = playerController.movespeed;
        playerController.movespeed = 0;
        yield return new WaitForSeconds(duration);
        playerController.movespeed = originSpeed;
        particle?.Stop();
        yield return new WaitForSeconds(cooldown);
        Destroy(nowItem);
    }

}