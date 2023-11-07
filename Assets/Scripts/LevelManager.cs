using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public bool IsGameOver;
    public GameObject WinUI;
    public GameObject Ground;
    public GameObject Grid;

    public Text timerText;

    public GameObject Path;

    public List<Transform> PathLocations;
    public GameObject NowLevelText;

    // Tilemap GroundMap;
    // Grid grid;

    private static Queue<int> EnemyToSummon;  // enemy ID
    private static Queue<Enemy> EnemyToRemove;  // enemy object

    private static List<EnemyLevelData> enemyLevelDatas;

    private int NowLevel;

    private bool inLevel;

    private float timer;

    void Awake() {
        Instance = this;

        // // get tilemap of ground
        // GroundMap = Ground.GetComponent<Tilemap>();

        // // get grid
        // grid = Grid.GetComponent<Grid>();

        IsGameOver = false;

        NowLevel = 0;

        inLevel = false;


        // get path locations
        Transform[] Locs = Path.GetComponentsInChildren<Transform>();

        for(int i = 1; i < Locs.Length; i ++) {
            PathLocations.Add(Locs[i]);
            // Debug.Log(Locs[i].position);
        }
        
        // initialize enemy summoner
        EnemySummon.Init();

        // initialize enemy queues
        EnemyToSummon = new Queue<int>();
        EnemyToRemove = new Queue<Enemy>();

    }

    // Start is called before the first frame update
    void Start()
    {

        // get all enemy level data
        EnemyLevelData[] temp = Resources.LoadAll<EnemyLevelData>("Enemies/EnemyLevelData");

        // add in list if active
        enemyLevelDatas = new List<EnemyLevelData>();
        foreach (EnemyLevelData enemyLevelData in temp) {
            if (enemyLevelData.isActive) {
                enemyLevelDatas.Add(enemyLevelData);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerText();
        // check whether LoadLevel is finished
        if (!inLevel) {
            if (NowLevel < enemyLevelDatas.Count)  {
                NowLevel ++;
                StartCoroutine(LoadLevel());
            } else {
                if (EnemySummon.EnemiesInGame.Count == 0) {
                    GameWin();
                }
            }
        } 
    }

    private void GameWin() {
        if (!IsGameOver) {
            Debug.Log("WIN");
            Application.Quit();
            Time.timeScale = 0;
            WinUI.SetActive(true);
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 100) % 100);

        timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    IEnumerator LoadLevel() {
        inLevel = true;

        EnemyLevelData enemyLevelData = enemyLevelDatas[NowLevel - 1];

        if (enemyLevelData == null) {
            Debug.Log("LevelManager.cs: enemyLevelData is null");
        }

        // if interval is 0, debug log and return
        if (enemyLevelData.spawnInterval == 0) {
            Debug.Log("LevelManager.cs: spawnInterval is 0");
        }

        // set timer to beforeSpawnInterval
        timer = enemyLevelData.beforeSpawnInterval;
        // set color of timer text to red
        timerText.color = Color.red;

        // while timer is not 0, update timer and timer text
        while (timer > 0) {
            timer -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(GameLoop());

        foreach (int enemyID in enemyLevelData.enemiesIDs) {
            EnqueEnemyToSummon(enemyID);

            yield return new WaitForSeconds(enemyLevelData.spawnInterval);
        }

        inLevel = false;

        yield return null;
    }

    IEnumerator GameLoop() {
        Debug.Log("Load Level " + NowLevel);
        NowLevelText.GetComponent<Text>().text = "Level: " + NowLevel;

        // set timer to 0
        timer = 0;
        // set color of timer text to black
        timerText.color = Color.black;

        // while game is not over
        while(!IsGameOver) {
            // EnemyLevelData enemyLevelData = enemyLevelDatas[NowLevel - 1];
            // StartCoroutine(SpawnEnemy(enemyLevelData));

            for (int i = 0; i < EnemyToSummon.Count; i++) {
                int enemyID = EnemyToSummon.Dequeue();
                EnemySummon.SummonEnemy(enemyID);
            }

            // remove enemies
            for (int i = 0; i < EnemyToRemove.Count; i ++) {
                Enemy enemyToRemove = EnemyToRemove.Dequeue();
                EnemySummon.RemoveEnemy(enemyToRemove);
            }

            // update timer
            timer += Time.deltaTime;

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
