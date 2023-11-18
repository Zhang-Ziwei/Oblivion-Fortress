using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Xml.Serialization;

public class EnemyBuff : MonoBehaviour 
{
    protected GameObject player;

    protected HeroKnight playerController;

    public float duration;

    private bool IsBuffed;

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

    private void Start() {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<HeroKnight>();
        isBuffed = false;
    }

    public void Buff() {

        if (isBuffed) {
            return;
        }
        DebuffLogList.Instance.AddBuffItem(this);

        StartCoroutine(BuffCoroutine());
    }

    public virtual IEnumerator BuffCoroutine() {
        yield return null;
    }

}