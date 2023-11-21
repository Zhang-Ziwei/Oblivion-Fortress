using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public static Castle Instance;
    public Slider healthBar;

    public float maxHealth;
    public float health;

    public GameObject GameOverUI;

    public AudioClip CastleHitAudio;

    public void GameOver() {
        Debug.Log("Game Over");
        Application.Quit();
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
    }

    public void DeductHealth(float damage) {
        health -= damage;
        healthBar.value = health / maxHealth;
        AudioSource.PlayClipAtPoint(CastleHitAudio, transform.position, 1);
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
