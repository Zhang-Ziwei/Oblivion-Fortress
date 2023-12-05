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

    public virtual void Init() {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<HeroKnight>();
    }

    public void Buff() {
        if (DebuffLogList.Instance.CheckDebuff(buffName)) {
            return;
        }


        nowItem = Instantiate(gameObject, player.transform.position, Quaternion.identity);

        DebuffLogList.Instance.AddBuffItem(this);

        // set the parent of the gameObject to player
        nowItem.transform.SetParent(player.transform);

        player.GetComponent<MonoBehaviour>().StartCoroutine(BuffCoroutine());
    }

    public virtual IEnumerator BuffCoroutine() {
        yield return null;
    }

}