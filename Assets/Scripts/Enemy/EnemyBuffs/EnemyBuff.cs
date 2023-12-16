using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuff : Buff
{
    public override void OnBuff(Enemy enemy) {
        base.OnBuff(enemy);
        
        nowItem = Instantiate(gameObject, enemy.transform.position, Quaternion.identity);

        particle = nowItem.GetComponent<ParticleSystem>();

        DebuffLogList.Instance.AddBuffItem(this);

        // set the parent of the gameObject to player
        nowItem.transform.SetParent(enemy.transform);

        enemy.GetComponent<MonoBehaviour>().StartCoroutine(BuffCoroutine());
    }    
}
