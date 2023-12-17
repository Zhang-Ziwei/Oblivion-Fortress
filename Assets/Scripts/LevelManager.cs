using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class EnemyAudio
{
    public GameObject audioObject;
    public AudioSource attacking_1;
    public AudioSource attacking_2;
    public AudioSource take_hit;
    public AudioSource death;

    public EnemyAudio(AudioClip attacking1Clip, AudioClip attacking2Clip, AudioClip takeHitClip, AudioClip deathClip, Transform transform, string name)
    {
        // Create an empty GameObject to hold the audio sources
        audioObject = new GameObject($"{name} EnemyAudio");

        audioObject.transform.SetParent(transform);

        // Add AudioSource components to the GameObject
        attacking_1 = audioObject.AddComponent<AudioSource>();
        attacking_2 = audioObject.AddComponent<AudioSource>();
        take_hit = audioObject.AddComponent<AudioSource>();
        death = audioObject.AddComponent<AudioSource>();

        // Assign the provided audio clips to the audio sources
        attacking_1.clip = attacking1Clip;
        attacking_2.clip = attacking2Clip;
        take_hit.clip = takeHitClip;
        death.clip = deathClip;
    }
    public void ApplySettings(float volume, float pitch, bool loop)
    {
        attacking_1.volume = volume;
        attacking_1.pitch = pitch;
        attacking_1.loop = loop;

        attacking_2.volume = volume;
        attacking_2.pitch = pitch;
        attacking_2.loop = loop;

        take_hit.volume = volume;
        take_hit.pitch = pitch;
        take_hit.loop = loop;

        death.volume = volume;
        death.pitch = pitch;
        death.loop = loop;
    }
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public bool IsGameOver;
    public GameObject WinUI;
    public GameObject Ground;
    public GameObject Grid;

    public Text timerText;

    public Text enemiesLeaveText;

    private static List<EnemyLevelData> enemyLevelDatas;

    private GameObject Path;

    private List<Transform> PathLocations;

    public List<Transform> GetPathLocations() {
        return PathLocations;
    }

    public GameObject NowLevelText;

    public AudioSource LevelStartAudio;
    public AudioSource BackgroundMusic;


    public Dictionary<string, EnemyAudio> enemyAudios;

    public bool isInfiniteMode = false;

    public List<int> randomGenerateEnemy;

    public float randomGenerateProbability = 0f;
    // Tilemap GroundMap;
    // Grid grid;

    private static Queue<(int, Vector3?)> EnemyToSummon;  // enemy ID
    private static Queue<Enemy> EnemyToRemove;  // enemy object

    private int NowLevel;

    private bool inLevel;

    private float timer;

    private int enemiesLeft;

    private Dictionary<string, int> Ends = new Dictionary<string, int>();

    void Awake() {
        Instance = this;

        IsGameOver = false;

        NowLevel = 0;

        inLevel = false;

        Path = GameObject.Find("Path");

        // get path locations
        Transform[] Locs = Path.GetComponentsInChildren<Transform>();
        PathLocations = new List<Transform>();
        for(int i = 1; i < Locs.Length; i ++) {
            PathLocations.Add(Locs[i]);
            // Debug.Log(Locs[i].position);
        }
        
        // initialize enemy summoner
        EnemySummon.Init();

        // initialize enemy queues
        EnemyToSummon = new Queue<(int, Vector3?)>();
        EnemyToRemove = new Queue<Enemy>();

        StartCoroutine(GameLoop());
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isInfiniteMode) {
            
            enemyLevelDatas = new List<EnemyLevelData>();

            int levelNum = 0;
            string[] difficulties = {"VeryEasy", "Easy", "Normal", "Hard"};
            foreach (string d in difficulties) {
                EnemyLevelData[] temp = Resources.LoadAll<EnemyLevelData>($"Enemies/EnemyLevelData/{gameObject.scene.name}/" + d);

                // add in list if active
                foreach (EnemyLevelData enemyLevelData in temp) {
                    if (enemyLevelData.isActive) {
                        enemyLevelDatas.Add(enemyLevelData);
                    }
                    levelNum ++;
                }
                Ends[d] = levelNum;
            }
        }
        else {
            // get all enemy level data
            EnemyLevelData[] temp = Resources.LoadAll<EnemyLevelData>($"Enemies/EnemyLevelData/{gameObject.scene.name}");

            // add in list if active
            enemyLevelDatas = new List<EnemyLevelData>();
            foreach (EnemyLevelData enemyLevelData in temp) {
                if (enemyLevelData.isActive) {
                    enemyLevelDatas.Add(enemyLevelData);
                }
            }
        }
        
        enemyAudios = new Dictionary<string, EnemyAudio>();

        // load audio
        foreach (string enemyName in EnemySummon.EnemyNames) 
        {
            AudioClip attacking_1 = Resources.Load<AudioClip>("Enemies/Audio/" + enemyName + "/attacking_1");
            AudioClip attacking_2 = Resources.Load<AudioClip>("Enemies/Audio/" + enemyName + "/attacking_2");
            AudioClip take_hit = Resources.Load<AudioClip>("Enemies/Audio/" + enemyName + "/take_hit");
            AudioClip death = Resources.Load<AudioClip>("Enemies/Audio/" + enemyName + "/death");

            EnemyAudio enemyAudio = new EnemyAudio(attacking_1, attacking_2, take_hit, death, transform, enemyName);

            // ensure all audio are loaded
            if (enemyAudio.attacking_1 == null || enemyAudio.attacking_2 == null || enemyAudio.take_hit == null || enemyAudio.death == null) {
                Debug.Log($"LevelManager.cs: enemy audio {enemyName} is null");
            } else {
                enemyAudios.Add(enemyName, enemyAudio);
            }
            
        }

    }   

    // Update is called once per frame
    void Update()
    {
        UpdateTimerText();
        UpdateEnemiesLeaveText();
        // check whether LoadLevel is finished
        if (!inLevel) {
            if (isInfiniteMode || NowLevel < enemyLevelDatas.Count)  {
                NowLevel ++;
                StartCoroutine(LoadLevel());
            } else {
                if (enemiesLeft <= 0) {
                    GameWin();
                }
            }
        } 

    }

    private void GameWin() {
        Time.timeScale = 0;
        WinUI.SetActive(true);
        BackgroundMusic.Stop();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 100) % 100);

        timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    private void UpdateEnemiesLeaveText() {
        enemiesLeft = EnemySummon.EnemiesInGame.Count;
        enemiesLeaveText.text =  enemiesLeft + " Enemies Leave";
    }

    IEnumerator LoadLevel() {
        inLevel = true;

        Debug.Log("Load Level " + NowLevel);
        //Debug.Log(" " + Ends["VeryEasy"] + Ends["Easy"] + Ends["Normal"] +Ends["Hard"]);
        EnemyLevelData enemyLevelData;
        if (isInfiniteMode) {

            // random summon
            if (NowLevel <= 5) {
                if (NowLevel <= 2)  enemyLevelData = enemyLevelDatas[UnityEngine.Random.Range(0, Ends["VeryEasy"])]; // summon Very Easy level
                else if (NowLevel == 3)  enemyLevelData = enemyLevelDatas[UnityEngine.Random.Range(Ends["VeryEasy"], Ends["Easy"])]; // summon Easy level
                else if (NowLevel == 4)  enemyLevelData = enemyLevelDatas[UnityEngine.Random.Range(Ends["Easy"], Ends["Normal"])]; // summon Normal level
                else enemyLevelData = enemyLevelDatas[UnityEngine.Random.Range(Ends["Normal"], Ends["Hard"])]; // summon Hard level
            }
            else {
                if (NowLevel % 5 == 0)  enemyLevelData = enemyLevelDatas[UnityEngine.Random.Range(Ends["Normal"], Ends["Hard"])]; // summon Hard level
                else if (NowLevel % 5 <= 2)  enemyLevelData = enemyLevelDatas[UnityEngine.Random.Range(Ends["VeryEasy"], Ends["Easy"])]; // summon Easy level
                else enemyLevelData = enemyLevelDatas[UnityEngine.Random.Range(Ends["Easy"], Ends["Normal"])]; // summon Normal level
            }

            // boost enemy every 5 level
            if (NowLevel >= 5 && NowLevel % 5 == 1) {
                if (NowLevel % 15 == 1) {
                    Difficulty.enemyDamageRate *= 1.5f;
                }
                else if (NowLevel % 15 == 6) {
                    Difficulty.enemyHealthRate *= 2;
                }
                else {
                    Difficulty.enemyAdditionLife += 1;
                }
            }
        }
        else {
            enemyLevelData = enemyLevelDatas[NowLevel - 1];

            // Increase enemy HP at level 6
            if (NowLevel == 6) Difficulty.enemyHealthRate *= 2f;
        }

        if (enemyLevelData == null) {
            Debug.Log("LevelManager.cs: enemyLevelData is null");
        }

        // if interval is 0, debug log and return
        if (enemyLevelData.spawnInterval == 0) {
            Debug.Log("LevelManager.cs: spawnInterval is 0");
        }

        // set timer to beforeSpawnInterval
        timer = enemyLevelData.beforeSpawnInterval * Difficulty.levelIntervalRate;
        // set color of timer text to red
        timerText.color = Color.red;

        timerText.gameObject.SetActive(true);

        // while timer is not 0, update timer and timer text
        while (timer > 0) {
            timer -= Time.deltaTime;
            yield return null;
        }

        // show level in UI
        NowLevelText.GetComponent<Text>().text = "Level: " + NowLevel;
        timer = 0;

        // level start audio
        LevelStartAudio.Play();

        // set timertext to inactive
        timerText.gameObject.SetActive(false);

        // gain exp
        gameObject.GetComponent<TowerUnlockManager>().GainExp(enemyLevelData.expGain);

        foreach (int enemyIndx in enemyLevelData.enemiesIDs) {
            EnqueEnemyToSummon(enemyIndx, null);
            yield return new WaitForSeconds(enemyLevelData.spawnInterval);
        }
        
        foreach (int enemyIndx in randomGenerateEnemy) {
            if (UnityEngine.Random.Range(0f, 1f) < randomGenerateProbability) {
                EnqueEnemyToSummon(enemyIndx, null);
                yield return new WaitForSeconds(enemyLevelData.spawnInterval);
            } 
        }

        inLevel = false;

        yield return null;
    }

    IEnumerator GameLoop() {
        // while game is not over
        while(!IsGameOver) {
            for (int i = 0; i < EnemyToSummon.Count; i++) {
                (int enemyID, Vector3? position) = EnemyToSummon.Dequeue();
                EnemySummon.SummonEnemy(enemyID, position);
            }

            // remove enemies
            for (int i = 0; i < EnemyToRemove.Count; i ++) {
                Enemy enemyToRemove = EnemyToRemove.Dequeue();
                EnemySummon.RemoveEnemy(enemyToRemove);
            }

            yield return null;
        }
    }

    public void EnqueEnemyToSummon(int enemyID, Vector3? position) {  // add new enemies to enemytosummon
        EnemyToSummon.Enqueue((enemyID, position));
    }
    public void EnqueEnemyToRemove(Enemy enemyToRemove) {  // add new enemies to enemytoremove
        EnemyToRemove.Enqueue(enemyToRemove);
    }
}
