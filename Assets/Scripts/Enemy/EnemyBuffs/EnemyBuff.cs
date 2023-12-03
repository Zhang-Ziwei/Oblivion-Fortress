using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class EnemyBuff : MonoBehaviour 
{
    protected HeroKnight playerController;

    protected GameObject player;

    public float duration;

    public float cooldown;

    private bool IsBuffed;

    public Sprite buffIcon;

    protected GameObject nowItem;

    protected string buffName;

    public string BuffName {
        get {
            return buffName;
        }
        set {
            buffName = value;
        }
    }

    // getter and setter
    public bool isBuffed {
        get {
            return IsBuffed;
        }
        set {
            IsBuffed = value;
        }
    }

    protected void Start() {
        isBuffed = false;
    }

    public virtual void Buff() {

    }

    public virtual IEnumerator BuffCoroutine() {
        yield return null;
    }

}