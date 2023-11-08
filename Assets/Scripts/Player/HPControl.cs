using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPControl : MonoBehaviour
{
    public float maxHP;
    public float HP;
    public Slider HPBar;
    public GameObject GameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        if (maxHP <= 0) {
            Debug.Log("maxHP should be positive");
            return;
        }

        HP = maxHP;
        HPBar.value = HP / maxHP;
    }

    public void GameOver() {
        Debug.Log("Game Over");
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
    }

    public void DeductHP(float damage) {
        HP -= damage;
        HPBar.value = HP / maxHP;

        if (HP <= 0) {
            GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
