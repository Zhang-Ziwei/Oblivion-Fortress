using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySummon : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static Dictionary<int, GameObject> EnemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;

    public static List<string> EnemyNames;

    public static void Init()
    {
        EnemyPrefabs = new Dictionary<int, GameObject>();

        // the design of this is to reduce the use of Instantiate
        EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();

        EnemiesInGame = new List<Enemy>();

        EnemySummonData[] enemies = Resources.LoadAll<EnemySummonData>("Enemies");

        EnemyNames = new List<string>();


        foreach (EnemySummonData enemy in enemies)
        {
            EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
            EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
            EnemyNames.Add(enemy.EnemyPrefab.name);
        }


    }

    public static Enemy SummonEnemy(int enemyID, Vector3? position)
    {
        Enemy SummonedEnemy = null;

        if (EnemyPrefabs.ContainsKey(enemyID)) {
            Queue<Enemy> ReferencedQueue = EnemyObjectPools[enemyID];


            // Debug.Log(ReferencedQueue.Count);
            if (ReferencedQueue.Count > 0) {
                // if there's already eenemies, dequeue and set active
                SummonedEnemy = ReferencedQueue.Dequeue();

                if (position != null) {
                    SummonedEnemy.transform.position = (Vector3)position;
                    SummonedEnemy.AssignNearestPath();
                    SummonedEnemy.gameObject.SetActive(true);
                    SummonedEnemy.Init(enemyID, position);

                } else {
                    SummonedEnemy.transform.position = LevelManager.Instance.GetPathLocations()[0].position;
                    SummonedEnemy.AssignNearestPath();
                    SummonedEnemy.gameObject.SetActive(true);
                    SummonedEnemy.Init(enemyID);
                }
                
                
            } else {
                if (position != null) {
                    GameObject NewEnemy = Instantiate(EnemyPrefabs[enemyID], (Vector3)position, Quaternion.identity);
                    SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                    SummonedEnemy.Init(enemyID, position);
                } else {
                    GameObject NewEnemy = Instantiate(EnemyPrefabs[enemyID], LevelManager.Instance.GetPathLocations()[0].position, Quaternion.identity);
                    SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                    SummonedEnemy.Init(enemyID);
                }
            }
            
            EnemiesInGame.Add(SummonedEnemy);
        } else {
            Debug.Log($"EntitySummoner.cs: Enemy ID {enemyID} does not exist.");
        }

        return SummonedEnemy;
    }

    public static void RemoveEnemy(Enemy enemyToRemove) {
        EnemiesInGame.Remove(enemyToRemove);

        // put the enemy back to the queue
        //EnemyObjectPools[enemyToRemove.GetID()].Enqueue(enemyToRemove);
        enemyToRemove.gameObject.SetActive(false);
    }
}
