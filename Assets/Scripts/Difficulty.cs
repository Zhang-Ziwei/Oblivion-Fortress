using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty: MonoBehaviour
{
    public int difficultyId = 0;
    public static float enemyHealthRate = 1f;
    public static float levelIntervalRate = 1f;
    public static bool DebugMode = false;

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
        Debug.Log(difficultyId);
    }

    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    // change frame color
    public void clickEasy(){
        GameObject.Find("EasyFrame").GetComponent<Image>().color = new Color32(170, 255, 170, 255);
        GameObject.Find("NormalFrame").GetComponent<Image>().color = new Color32(50, 120, 50, 255);
        GameObject.Find("HardFrame").GetComponent<Image>().color = new Color32(50, 120, 50, 255);
    }
    
    public void clickNormal(){
        GameObject.Find("EasyFrame").GetComponent<Image>().color = new Color32(50, 120, 50, 255);
        GameObject.Find("NormalFrame").GetComponent<Image>().color = new Color32(170, 255, 170, 255);
        GameObject.Find("HardFrame").GetComponent<Image>().color = new Color32(50, 120, 50, 255);
    }

    public void clickHard(){
        GameObject.Find("EasyFrame").GetComponent<Image>().color = new Color32(50, 120, 50, 255);
        GameObject.Find("NormalFrame").GetComponent<Image>().color = new Color32(50, 120, 50, 255);
        GameObject.Find("HardFrame").GetComponent<Image>().color = new Color32(170, 255, 170, 255);
    }
}
