using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLevelData", menuName = "ScriptableObjects/EnemyLevelData")]
public class EnemyLevelData: ScriptableObject
{
    public List<int> enemiesIDs;
    public float spawnInterval;

    private int enemiesLeft;

    // define getter and setter for enemiesLeft
    public int EnemiesLeft
    {
        get { return enemiesLeft; }
        set { enemiesLeft = value; }
    }
}
