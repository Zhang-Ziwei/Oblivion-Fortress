using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : EnemyBuff
{
    public int summonID;
    public int summonCount;
    public float summonRange;

    public override void OnBuff(Enemy enemy) {
        buffName = "Summon";
        if (DebuffLogList.Instance.CheckDebuff(buffName)) {
            return;
        }
        base.OnBuff(enemy);
        // set gameobject rotation x to 15
        nowItem.transform.rotation = Quaternion.Euler(15, 0, 0);
    }

    public override IEnumerator BuffCoroutine() {
        List<GameObject> summonCircles = new List<GameObject>();
        float summonInterval = duration / summonCount;
        for (int i = 0; i < summonCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            Enemy summonedEnemy = EnemySummon.SummonEnemy(summonID, randomPosition);
            
            GameObject summonCircle = Instantiate(gameObject, summonedEnemy.transform.position, Quaternion.identity);
            summonCircle.transform.rotation = Quaternion.Euler(15, 0, 0);
            // apply the same buff to the summoned enemy
            summonCircles.Add(summonCircle);

            summonCircle.transform.SetParent(summonedEnemy.transform);
            
            yield return new WaitForSeconds(summonInterval);
        }
        if (nowItem != null)
        {
            particle?.Stop();
        }
        foreach (GameObject summonCircle in summonCircles)
        {
            Destroy(summonCircle);
        }
        Destroy(nowItem);
        yield return new WaitForSeconds(cooldown);
    }

    Vector3 GetRandomPosition() {
        Vector3 randomPosition = new Vector3(Random.Range(-summonRange, summonRange), Random.Range(-summonRange, summonRange), 0);
            

        return enemy.GetOnGround(enemy.transform.position + randomPosition);
    }
}
