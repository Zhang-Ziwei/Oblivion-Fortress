using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;



// public class EnemyBuffCopy : MonoBehaviour 
// {
//     private GameObject player;
//     private PlayerController playerController;

//     private void Start() {
//         player = GameObject.Find("Player");
//         playerController = player.GetComponent<PlayerController>();
//     }


    // public void Slowness(object slownessData, GameObject player) {
    //     if (slownessData is SlownessData) {
    //         SlownessData slownessDataNew = (SlownessData)slownessData;
    //         // SlownessData slownessData = (SlownessData)enemyBuffData;
    //         if (slownessDataNew == null) {
    //             Debug.Log("slownessData is null");
    //         }
    //         if (slownessDataNew.IsBuffed) {
    //             return;
    //         }
            
    //         if (playerController == null) {
    //             Debug.Log("playerController is null");
    //         } else {
    //             StartCoroutine(SlownessCoroutine(slownessDataNew, playerController));
    //         }
    //     } 
    // }

    // public IEnumerator SlownessCoroutine(SlownessData slownessData, PlayerController playerController)
    // {
    //     slownessData.IsBuffed = true;
    //     float originSpeed = playerController.movespeed;
    //     playerController.movespeed = originSpeed * slownessData.ratio;
    //     yield return new WaitForSeconds(slownessData.duration);
    //     playerController.movespeed = originSpeed;
    //     slownessData.IsBuffed = false;
    // }

    // public void Poison(object poisonData, GameObject player) {
    //     if (poisonData is PoisonData) {
    //         PoisonData poisonDataNew = (PoisonData)poisonData;
    //         // PoisonData poisonData = (PoisonData)enemyBuffData;
    //         if (poisonDataNew == null) {
    //             Debug.Log("poisonData is null");
    //         }

    //         if (poisonDataNew.IsBuffed) {
    //             return;
    //         }
    //         HPControl playerHP = player.GetComponent<HPControl>();
    //         StartCoroutine(PoisonCoroutine(poisonDataNew, playerHP));
    //         }
    // }
//     public IEnumerator PoisonCoroutine(PoisonData poisonData, HPControl playerHP)
//     {
//         poisonData.IsBuffed = true;
//         float timer = 0;
//         while (timer < poisonData.duration)
//         {
//             timer += poisonData.interval;
//             playerHP.DeductHP(poisonData.damage);
//             yield return new WaitForSeconds(poisonData.interval);
//         }
//         poisonData.IsBuffed = false;

//     }
// }