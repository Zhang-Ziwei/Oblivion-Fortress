using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : EnemyBuff
{
    public float healAmount;

    public float interval;

    public float healRange;

    public override void OnBuff(Enemy enemy) {
        buffName = "Healing";
        if (DebuffLogList.Instance.CheckDebuff(buffName)) {
            return;
        }
        base.OnBuff(enemy);
    }

    public override IEnumerator BuffCoroutine() {
        // find enemy within range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(nowItem.transform.position, healRange);
        List<Enemy> nearbyEnemies = new List<Enemy>();
        List<GameObject> healthBuffs = new List<GameObject>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                // exclude the enemy itself
                if (collider.gameObject == enemy.gameObject)
                {
                    continue;
                }
                Enemy nowEnemy = collider.gameObject.GetComponent<Enemy>();
                nearbyEnemies.Add(nowEnemy);
                GameObject healthBuff = Instantiate(gameObject, nowEnemy.transform.position, Quaternion.identity);
                healthBuff.transform.SetParent(nowEnemy.transform);
                healthBuffs.Add(healthBuff);
            }
        }
        float timer = 0;
        while (timer < duration)
        {
            timer += interval;
            foreach (Enemy nearbyEnemy in nearbyEnemies)
            {
                nearbyEnemy.RecoverHealth(healAmount);
            }
            yield return new WaitForSeconds(interval);
        }
        if (nowItem != null)
        {
            particle?.Stop();
        }
        foreach (GameObject healthBuff in healthBuffs)
        {
            Destroy(healthBuff);
        }
        Destroy(nowItem);
        yield return new WaitForSeconds(cooldown);

    }
}
