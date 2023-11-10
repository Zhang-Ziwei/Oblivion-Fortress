using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public class EnemyBuffData : ScriptableObject
{
    public int ID;
    protected bool isBuffed = false;

    public bool IsBuffed {
        get {
            return isBuffed;
        } 
        set {
            isBuffed = value;
        }
    }

    // constructor
    public EnemyBuffData() {

    }
}



