using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class Slowness: EnemyBuff
{
    public float ratio;

    private new void Start() {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<HeroKnight>();
        isBuffed = false;
        buffName = "Slowness";

        if (player == null) {
            Debug.Log("player is null");
        }
        if (playerController == null) {
            Debug.Log("playerController is null");
        }
        if (ratio <= 0) {
            Debug.Log("ratio is not positive");
        }
    }

    public override IEnumerator BuffCoroutine() {
        isBuffed = true;
        float originSpeed = playerController.movespeed;
        playerController.movespeed = originSpeed * ratio;
        yield return new WaitForSeconds(duration);
        playerController.movespeed = originSpeed;

        yield return new WaitForSeconds(cooldown);
        isBuffed = false;
    }
}