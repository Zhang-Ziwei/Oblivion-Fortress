using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class Buff : MonoBehaviour 
{

    public float duration;

    public float cooldown;

    private bool IsBuffed;

    public Sprite buffIcon;

    protected GameObject nowItem;

    protected string buffName;

    protected Enemy enemy;

    protected ParticleSystem particle;

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
    public virtual void OnBuff(Enemy enemy) {
        this.enemy = enemy;   
    }

    public virtual IEnumerator BuffCoroutine() {
        yield return null;
    }

}