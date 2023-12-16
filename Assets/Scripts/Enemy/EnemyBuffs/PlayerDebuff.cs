using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebuff : Buff
{
    protected HeroKnight playerController;

    protected GameObject player;

    public override void OnBuff(Enemy enemy) {
        base.OnBuff(enemy);

        player = enemy.Player;
        playerController = player.GetComponent<HeroKnight>();


        nowItem = Instantiate(gameObject, player.transform.position, Quaternion.identity);

        particle = nowItem.GetComponent<ParticleSystem>();
        DebuffLogList.Instance.AddBuffItem(this);

        // set the parent of the gameObject to player
        nowItem.transform.SetParent(player.transform);

        player.GetComponent<MonoBehaviour>().StartCoroutine(BuffCoroutine());
    }

    public override IEnumerator BuffCoroutine() {
        yield return null;
    }
}
