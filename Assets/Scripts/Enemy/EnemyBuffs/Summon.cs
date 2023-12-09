using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : EnemyBuff
{
    public int summonID;
    public int summonCount;
    public float summonInterval;
    public float summonRange;

    public override void OnBuff(Enemy enemy) {
        buffName = "Summon";
        base.OnBuff(enemy);
    }

    public override IEnumerator BuffCoroutine() {
        List<GameObject> summonCircles = new List<GameObject>();
        for (int i = 0; i < summonCount; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-summonRange, summonRange), Random.Range(-summonRange, summonRange), 0);
            Enemy summonedEnemy = EnemySummon.SummonEnemy(summonID, enemy.transform.position + randomPosition);
            
            GameObject summonCircle = Instantiate(gameObject, summonedEnemy.transform.position, Quaternion.identity);
            // apply the same buff to the summoned enemy
            summonCircles.Add(summonCircle);

            summonCircle.transform.SetParent(summonedEnemy.transform);
            
            yield return new WaitForSeconds(summonInterval);
        }
        particle?.Stop();

        yield return new WaitForSeconds(cooldown);
        Destroy(nowItem);

        foreach (GameObject summonCircle in summonCircles)
        {
            Destroy(summonCircle);
        }
    }
}
