using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public bool IsGameOver;

    public GameObject Ground;
    public GameObject Grid;

    public GameObject Path;

    public List<Transform> PathLocations;

    // Tilemap GroundMap;
    // Grid grid;

    private static Queue<int> EnemyToSummon;  // enemy ID
    private static Queue<Enemy> EnemyToRemove;  // enemy object

    private static EnemyLevelData[] enemyLevelDatas;

    private int NowLevel;

    void Awake() {
        Instance = this;

        // // get tilemap of ground
        // GroundMap = Ground.GetComponent<Tilemap>();

        // // get grid
        // grid = Grid.GetComponent<Grid>();

        IsGameOver = false;

        NowLevel = 1;


        // get path locations
        Transform[] Locs = Path.GetComponentsInChildren<Transform>();

        for(int i = 1; i < Locs.Length; i ++) {
            PathLocations.Add(Locs[i]);
            // Debug.Log(Locs[i].position);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
        // initialize enemy summoner
        EnemySummon.Init();

        // initialize enemy queues
        EnemyToSummon = new Queue<int>();
        EnemyToRemove = new Queue<Enemy>();

        // get all enemy level data
        enemyLevelDatas = Resources.LoadAll<EnemyLevelData>("Enemies/EnemyLevelData");

        // reset enemies left
        foreach (EnemyLevelData enemyLevelData in enemyLevelDatas) {
            enemyLevelData.EnemiesLeft = enemyLevelData.enemiesIDs.Count;
        }

        StartCoroutine(LoadLevel());

    }

    // Update is called once per frame
    void Update()
    {
        // check whether LoadLevel is finished
        if (enemyLevelDatas[NowLevel - 1].EnemiesLeft == 0 && EnemySummon.EnemiesInGame.Count == 0) {
            if (NowLevel == enemyLevelDatas.Length) {
                IsGameOver = true;
                Debug.Log("Game Over");
                Application.Quit();
            } else {
                Debug.Log("Load Next Level");
                NowLevel ++;
                StartCoroutine(LoadLevel());
            }
        }
    }

    IEnumerator LoadLevel() {
        EnemyLevelData enemyLevelData = enemyLevelDatas[NowLevel - 1];

        if (enemyLevelData == null) {
            Debug.Log("LevelManager.cs: enemyLevelData is null");
        }

        // if interval is 0, debug log and return
        if (enemyLevelData.spawnInterval == 0) {
            Debug.Log("LevelManager.cs: spawnInterval is 0");
        }

        StartCoroutine(GameLoop());

        foreach (int enemyID in enemyLevelData.enemiesIDs) {
            EnqueEnemyToSummon(enemyID);
            // decrease enemies left
            enemyLevelData.EnemiesLeft --;
            yield return new WaitForSeconds(enemyLevelData.spawnInterval);
        }

        yield return null;
    }

    IEnumerator GameLoop() {
        // while game is not over
        while(!IsGameOver) {
            // EnemyLevelData enemyLevelData = enemyLevelDatas[NowLevel - 1];
            // StartCoroutine(SpawnEnemy(enemyLevelData));

            for (int i = 0; i < EnemyToSummon.Count; i++) {
                int enemyID = EnemyToSummon.Dequeue();
                EnemySummon.SummonEnemy(enemyID);
            }
            // move the enemies


            // towers


            // damage enemies

            // remove enemies
            for (int i = 0; i < EnemyToRemove.Count; i ++) {
                Enemy enemyToRemove = EnemyToRemove.Dequeue();
                EnemySummon.RemoveEnemy(enemyToRemove);
            }

            yield return null;
        }
    }

    void EnqueEnemyToSummon(int enemyID) {  // add new enemies to enemytosummon
        EnemyToSummon.Enqueue(enemyID);
    }
    void EnqueEnemyToRemove(Enemy enemyToRemove) {  // add new enemies to enemytoremove
        EnemyToRemove.Enqueue(enemyToRemove);
    }
}
