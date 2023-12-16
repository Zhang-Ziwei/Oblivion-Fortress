using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class HPControl : MonoBehaviour
{
    public int PenaltyTime = 10;
    public float maxHP;
    public float HP;

    public Slider HPBar;

    [SerializeField] private HurtUI hurtUI;

    private GameObject player;
    private bool die;
    private float AccuPT = 0f;

    private Animator m_animator;

    private bool PauseEnable = false;
    public GameObject pauseUI;
    private int toolstype = 0;



    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();

        if (maxHP <= 0) {
            Debug.Log("maxHP should be positive");
            return;
        }
        die = false;
        HP = maxHP;
        HPBar.value = HP / maxHP;
    }

    public void DeductHP(float damage, bool isCritical = false, float delayTime = 0f) {
        /*
        if(toolstype == 0)
        {
            m_animator.SetTrigger("Hurt");
        }
        if (toolstype == 1)
        {
            m_animator.SetTrigger("Hurt_ax");
        }
        if (toolstype == 2)
        {
            m_animator.SetTrigger("Hurt_ham");
        }
        */
        HP -= damage;
        if (isCritical) {
            hurtUI.Init(damage, transform, true);
        } else {
            hurtUI.Init(damage, transform, false);
        }

        if (HP <= 0) {
            Debug.Log("Died");
            die = true;
            AccuPT = 0;
            if (toolstype == 0)
                m_animator.SetTrigger("Death");
            if (toolstype == 1)
                m_animator.SetTrigger("Death_ax");
            if (toolstype == 2)
                m_animator.SetTrigger("Death_ham");
            if (toolstype == 3)
                m_animator.SetTrigger("Death_wood");
            

            // blocking player control
            GetComponent<HeroKnight>().enabled = false;
            GetComponent<PickupSystem>().enabled = false;
            GetComponent<CollectResource>().enabled = false;

            Invoke("Relive",PenaltyTime);
        }
    }

    public void Relive(){
        Debug.Log("Relive");
        die = false;

        // unblock player control
        GetComponent<HeroKnight>().enabled = true;
        GetComponent<PickupSystem>().enabled = true;
        GetComponent<CollectResource>().enabled = true;

        HP = maxHP;
        PenaltyTime += 10; // relive time +10s
        m_animator.SetTrigger("Hurt"); // wake up
    }

    public void RecoverHP(float value) {
        HP = Math.Min(maxHP, HP + value);
    }

    void pausegame()
    {
        //pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseEnable == false)
            {
                PauseEnable = true;
                Time.timeScale = 0;
                pauseUI.SetActive(true);
            }
            else if (PauseEnable == true)
            {
                PauseEnable = false;
                Time.timeScale = 1;
                pauseUI.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        pausegame();

        toolstype = GetComponent<PickupSystem>().type;
        if (!(die)){
            HPBar.value = HP / maxHP;
        }
        else{
            AccuPT += Time.deltaTime;
            HPBar.value = AccuPT / PenaltyTime;
        }
    }
}
