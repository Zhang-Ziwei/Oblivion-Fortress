using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class Freeze: EnemyBuff
{
    private new void Start() {
        base.Start();
        buffName = "Freeze";

    }

    public override IEnumerator BuffCoroutine() {
        isBuffed = true;
        float originSpeed = playerController.movespeed;
        playerController.movespeed = 0;
        yield return new WaitForSeconds(duration);
        playerController.movespeed = originSpeed;
        yield return new WaitForSeconds(cooldown);
        isBuffed = false;
    }

    // void Update() {
    //     Debug.Log(playerController.movespeed);
    // }
}