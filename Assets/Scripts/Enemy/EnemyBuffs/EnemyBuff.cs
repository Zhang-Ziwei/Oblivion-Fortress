using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class EnemyBuff : MonoBehaviour 
{
    protected GameObject player;

    protected PlayerController playerController;

    private bool IsBuffed;

    // getter and setter
    public bool isBuffed {
        get {
            return IsBuffed;
        }
        set {
            IsBuffed = value;
        }
    }

    private UnityEvent nowEvent;

    public UnityEvent NowEvent {
        get {
            return nowEvent;
        }
        set {
            nowEvent = value;
        }
    }

    private void Start() {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        nowEvent = new UnityEvent();
    }

    public void Buff() {
        if (isBuffed) {
            return;
        }
        StartCoroutine(BuffCoroutine());
    }

    public virtual IEnumerator BuffCoroutine() {
        yield return null;
    }

}