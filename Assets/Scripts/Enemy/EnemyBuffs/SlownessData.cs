using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


// inherit from EnemyBuffData
[CreateAssetMenu(fileName = "SlownessData", menuName = "ScriptableObjects/EnemyBuffData/SlownessData")]
public class SlownessData : EnemyBuffData
{
    public float ratio;

    public float duration;
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
    public SlownessData() {

    }
}