using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty: MonoBehaviour
{
    public int difficultyId = 0;
    public static float enemyHealthRate = 1f;
    public static float levelIntervalRate = 1f;
    //private Button button;
    // Start is called before the first frame update
    public void SetDifficulty(int id){
        if (id == 0){ //easy
            enemyHealthRate = 1f;
            levelIntervalRate = 1f;
        }
        else if (id == 1){ //normal
            enemyHealthRate = 1.5f;
            levelIntervalRate = 0.9f;
        }
        else if (id == 2){ //hard
            enemyHealthRate = 2f;
            levelIntervalRate = 0.8f;
        }
    }

    public void TaskOnClick()
    {
        SetDifficulty(difficultyId);
    }

    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
