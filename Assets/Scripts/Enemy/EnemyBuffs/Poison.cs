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

    private HPControl playerHP;

    private void Start() {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<HeroKnight>();
        isBuffed = false;

        if (player == null) {
            Debug.Log("player is null");
        }
        if (playerController == null) {
            Debug.Log("playerController is null");
        }
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