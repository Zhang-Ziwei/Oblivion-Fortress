using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

// inherit from EnemyBuffData
[CreateAssetMenu(fileName = "PoisonData", menuName = "ScriptableObjects/EnemyBuffData/PoisonData")]

public class PoisonData :  EnemyBuffData
{
    public float damage;

    public float duration;

    public float interval;
    // protected bool isBuffed = false;

    // public bool IsBuffed {
    //     get {
    //         return isBuffed;
    //     } 
    //     set {
    //         isBuffed = value;
    //     }
    // }
    // constructor
    public PoisonData() {

    }
}