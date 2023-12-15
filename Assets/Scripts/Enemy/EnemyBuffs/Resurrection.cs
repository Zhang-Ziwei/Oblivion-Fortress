using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrection : EnemyBuff
{
    private float interval = 0.1f;

    public override void OnBuff(Enemy enemy) {
        buffName = "Resurrection";
        if (enemy.life <= 0) {
            return;
        }
        if (DebuffLogList.Instance.CheckDebuff(buffName)) {
            return;
        }

        base.OnBuff(enemy);
        // set gameobject rotation x to 15
        nowItem.transform.rotation = Quaternion.Euler(15, 0, 0);
    }

    public override IEnumerator BuffCoroutine() {
        enemy.IsResurrected = true;
        enemy.ActionMode = -2;
        float multiplier = duration / interval;
        float healthToAdd = enemy.maxHealth / multiplier;

        while (enemy.Health < enemy.maxHealth)
        {
            enemy.RecoverHealth(healthToAdd);
            yield return new WaitForSeconds(interval);
        }
        if (nowItem != null)
        {
            particle?.Stop();
        }
        enemy.IsResurrected = false;
        enemy.ActionMode = 1;

        yield return new WaitForSeconds(cooldown);
        Destroy(nowItem);
    }
}
