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

    Tilemap GroundMap;
    Grid grid;

    private static Queue<int> EnemyToSummon;  // enemy ID
    private static Queue<Enemy> EnemyToRemove;  // enemy object

    void Awake() {
        Instance = this;

        // get tilemap of ground
        GroundMap = Ground.GetComponent<Tilemap>();

        // get grid
        grid = Grid.GetComponent<Grid>();

        IsGameOver = false;


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


        EnemyToSummon.Enqueue(1);

        // start game loop
        StartCoroutine(GameLoop());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GameLoop() {
        // while game is not over
        while(!IsGameOver) {
            // spawn enemies
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
