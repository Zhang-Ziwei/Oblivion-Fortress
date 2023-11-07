using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySummon : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static Dictionary<int, GameObject> EnemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;

    private static bool isInitialized = false;

    public static void Init()
    {
        if (!isInitialized)
        {
            EnemyPrefabs = new Dictionary<int, GameObject>();

            // the design of this is to reduce the use of Instantiate
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();

            EnemiesInGame = new List<Enemy>();

            EnemySummonData[] enemies = Resources.LoadAll<EnemySummonData>("Enemies");


            foreach (EnemySummonData enemy in enemies)
            {
                EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
                EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
            }
            isInitialized = true;
        }
    }

    public static Enemy SummonEnemy(int enemyID)
    {
        Enemy SummonedEnemy = null;

        if (EnemyPrefabs.ContainsKey(enemyID)) {
            Queue<Enemy> ReferencedQueue = EnemyObjectPools[enemyID];


            // Debug.Log(ReferencedQueue.Count);
            if (ReferencedQueue.Count > 0) {
                // if there's already eenemies, dequeue and set active
                SummonedEnemy = ReferencedQueue.Dequeue();

                Debug.Log(SummonedEnemy);

                SummonedEnemy.gameObject.SetActive(true);
                
            } else {

                // if no, instantiate
                GameObject NewEnemy = Instantiate(EnemyPrefabs[enemyID], LevelManager.Instance.PathLocations[0].position, Quaternion.identity);
                SummonedEnemy = NewEnemy.GetComponent<Enemy>();
            }
            SummonedEnemy.Init(enemyID);
            
            EnemiesInGame.Add(SummonedEnemy);
        } else {
            Debug.Log($"EntitySummoner.cs: Enemy ID {enemyID} does not exist.");
        }

        return SummonedEnemy;
    }

    public static void RemoveEnemy(Enemy enemyToRemove) {
        EnemiesInGame.Remove(enemyToRemove);

        // put the enemy back to the queue
        EnemyObjectPools[enemyToRemove.GetID()].Enqueue(enemyToRemove);
        enemyToRemove.gameObject.SetActive(false);
    }
}
