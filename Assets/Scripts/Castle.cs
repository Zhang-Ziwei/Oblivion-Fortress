using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public static Castle Instance;
    public Slider healthBar;

    public float maxHealth;
    private float health;

    public GameObject GameOverUI;

    public void GameOver() {
        Debug.Log("Game Over");
        Time.timeScale = 0;
        GameOverUI.SetActive(true);

    }

    public void DeductHealth(float damage) {
        health -= damage;
        healthBar.value = health / maxHealth;

        if (health <= 0) {
            GameOver();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

        if (maxHealth <= 0) {
            Debug.Log("maxHealth should be positive");
            return;
        }

        health = maxHealth;
        healthBar.value = health / maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
