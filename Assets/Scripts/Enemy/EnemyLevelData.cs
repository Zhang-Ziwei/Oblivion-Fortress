using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLevelData", menuName = "ScriptableObjects/EnemyLevelData")]
public class EnemyLevelData: ScriptableObject
{
    public List<int> enemiesIDs;
    public float spawnInterval;

    public float beforeSpawnInterval;

    public bool isActive;

    private float timer;

    // getter and setter of timer
    public float Timer
    {
        get { return timer; }
        set { timer = value; }
    }

}
