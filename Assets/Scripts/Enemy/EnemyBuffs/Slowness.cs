using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class Slowness: EnemyBuff
{
    public float ratio;

    public float duration;

    public override IEnumerator BuffCoroutine() {
        isBuffed = true;
        float originSpeed = playerController.movespeed;
        playerController.movespeed = originSpeed * ratio;
        yield return new WaitForSeconds(duration);
        playerController.movespeed = originSpeed;
        isBuffed = false;
    }
}