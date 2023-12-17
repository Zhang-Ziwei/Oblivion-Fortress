using UnityEngine;
using System.Collections;
using System;

public class HeroKnight : MonoBehaviour {

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private int toolstype = 0;

    // walk and rush in playcontroller
    public float movespeed = 2f;
    public Camera maincamera;
    private Animation animate;
    private int update_totaltime = 100;
    private bool isRush = false;

    private float rushTime = 0.5f;
    private float rushTimer = 0;
    private float rushCoolDownTimer = 0;   
    private bool isSpeedUp = false;

    public float AttackRange;

    public float Damage;
    public float AttackInterval = 0.5f;


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // walk and rush in playcontroller
    private void rush(int toolstype)
    {
        rushTimer -= Time.deltaTime;
        rushCoolDownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && isRush == false && !Input.GetMouseButton(0))
        {
            //update_temptime = update_totaltime;
            rushTimer = rushTime / 3 * 2;
            rushCoolDownTimer = rushTime;
            movespeed = movespeed * 3;
            isRush = true;
            isSpeedUp = true;
            if(toolstype == 0)
                m_animator.SetTrigger("Roll");
            if (toolstype == 1)
                m_animator.SetTrigger("Roll_ax");
                m_animator.SetBool("Attack_stop", false);
            if (toolstype == 2)
                m_animator.SetTrigger("Roll_ham");
                m_animator.SetBool("Attack_stop", false);
            if (toolstype == 3)
                m_animator.SetTrigger("Roll_wood");
            if (toolstype == 4)
                m_animator.SetTrigger("Roll_stone");
        }
        if (isSpeedUp && (rushTimer < 0))//update_totaltime - update_temptime == rush_cyclenum)
        {
            movespeed = movespeed / 3;
            isSpeedUp = false;
        }
        if (isRush && (rushCoolDownTimer < 0))//update_totaltime - update_temptime == rush_cyclenum + 50)
        {
            isRush = false;
        }
    }

    private void Move(int toolstype)
    {
        // UnityEngine.Debug.Log(toolstype);
        Vector2 dir = Vector2.zero;


        if (Input.GetKey(KeyCode.D) && !Input.GetKeyDown(KeyCode.E))
        {
            dir += new Vector2(1, 0);
            //transform.Translate(movespeed * Time.deltaTime, 0, 0);
            if (toolstype == 0)
                m_animator.SetBool("Run", true);
            if (toolstype == 1)
            {
                m_animator.SetBool("Run_ax", true);
                m_animator.SetBool("axe", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 2)
            {
                m_animator.SetBool("Run_ham", true);
                m_animator.SetBool("ham", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 3)
            {
                m_animator.SetBool("Run_wood", true);
                m_animator.SetBool("wood", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 4)
            {
                m_animator.SetBool("Run_stone", true);
                m_animator.SetBool("stone", false);
                m_animator.SetBool("Run", false);
            }
        }
        if (Input.GetKey(KeyCode.A) && !Input.GetKeyDown(KeyCode.E))
        {
            dir += new Vector2(-1, 0);
            //transform.Translate(-movespeed * Time.deltaTime, 0, 0);
            if (toolstype == 0)
                m_animator.SetBool("Run", true);
            if (toolstype == 1) { 
                m_animator.SetBool("Run_ax", true);
                m_animator.SetBool("axe", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 2)
            {
                m_animator.SetBool("Run_ham", true);
                m_animator.SetBool("ham", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 3)
            {
                m_animator.SetBool("Run_wood", true);
                m_animator.SetBool("wood", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 4)
            {
                m_animator.SetBool("Run_stone", true);
                m_animator.SetBool("stone", false);
                m_animator.SetBool("Run", false);
            }
        }
        if (Input.GetKey(KeyCode.W) && !Input.GetKeyDown(KeyCode.E))
        {
            dir += new Vector2(0, 1);
            //transform.Translate(0, movespeed * Time.deltaTime, 0);
            if (toolstype == 0)
                m_animator.SetBool("Run", true);
            if (toolstype == 1) { 
                m_animator.SetBool("Run_ax", true);
                m_animator.SetBool("axe", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 2)
            {
                m_animator.SetBool("Run_ham", true);
                m_animator.SetBool("ham", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 3)
            {
                m_animator.SetBool("Run_wood", true);
                m_animator.SetBool("wood", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 4)
            {
                m_animator.SetBool("Run_stone", true);
                m_animator.SetBool("stone", false);
                m_animator.SetBool("Run", false);
            }
        }
        if (Input.GetKey(KeyCode.S) && !Input.GetKeyDown(KeyCode.E))
        {
            dir += new Vector2(0, -1);
            //transform.Translate(0, -movespeed * Time.deltaTime, 0);
            if (toolstype == 0)
                m_animator.SetBool("Run", true);
            if (toolstype == 1) { 
                m_animator.SetBool("Run_ax", true);
                m_animator.SetBool("axe", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 2)
            {
                m_animator.SetBool("Run_ham", true);
                m_animator.SetBool("ham", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 3)
            {
                m_animator.SetBool("Run_wood", true);
                m_animator.SetBool("wood", false);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 4)
            {
                m_animator.SetBool("Run_stone", true);
                m_animator.SetBool("stone", false);
                m_animator.SetBool("Run", false);
            }
        }
        m_body2d.MovePosition(m_body2d.position + dir.normalized * movespeed * Time.deltaTime);
        if(!Input.GetKey(KeyCode.D)&& !Input.GetKey(KeyCode.A)&& !Input.GetKey(KeyCode.W)&& !Input.GetKey(KeyCode.S) && !Input.GetMouseButton(0))
        {
            if (toolstype == 0)
                m_animator.SetBool("Run", false);
            if (toolstype == 1) { 
                m_animator.SetBool("Run_ax", false);
                //m_animator.SetBool("axe", true);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 2)
            {
                m_animator.SetBool("Run_ham", false);
                //m_animator.SetBool("ham", true);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 3)
            {
                m_animator.SetBool("Run_wood", false);
                //m_animator.SetBool("ham", true);
                m_animator.SetBool("Run", false);
            }
            if (toolstype == 4)
            {
                m_animator.SetBool("Run_stone", false);
                //m_animator.SetBool("ham", true);
                m_animator.SetBool("Run", false);
            }
        }
    }

    private void LateUpdate()
    {
        if (maincamera != null)
        {
            maincamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2.0f);
        }
    }

    // walk and rush in playcontroller


    // detect whether the player hurt the enemy when attack
    private void HurtEnemy() {
        // if we could find the enemy in the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, AttackRange);
        foreach (Collider2D enemy in hitEnemies) {
            if (enemy.tag == "Enemy") {
                // hurt the enemy
                enemy.GetComponent<Enemy>().DeductHealth(Damage * Math.Max(Difficulty.enemyHealthRate, 1f));
            }
        }
    }

    void FixedUpdate(){
        Move(toolstype);
    }
    // Update is called once per frame
    void Update ()
    {
        // walk and rush in playcontroller
        update_totaltime++;
        if(Input.GetKey(KeyCode.E)){
            toolstype = GetComponent<PickupSystem>().type;
        }
        //Move(toolstype);
        rush(toolstype);

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Attack
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > AttackInterval && toolstype == 0)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // hurt enemy
            HurtEnemy();

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        //Attack_ax
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > AttackInterval && toolstype == 1)
        {
            m_currentAttack++;
            m_animator.SetBool("Attack_stop", false);
            m_animator.SetBool("axe", false);
            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack + "_ax");

            // hurt enemy
            HurtEnemy();

            // Reset timer
            m_timeSinceAttack = 0.0f;

            // delay the attack when hero did not move
            //Invoke("delayOpen", 2f); //5秒后调用 delayOpen () 函数  ，只调用一次  do not need to goto hero_ax again
        }

        //Attack_ham
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > AttackInterval && toolstype == 2)
        {
            m_currentAttack++;
            m_animator.SetBool("Attack_stop", false);
            m_animator.SetBool("ham", false);
            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack + "_ham");

            // hurt enemy
            HurtEnemy();

            // Reset timer
            m_timeSinceAttack = 0.0f;

            // delay the attack when hero did not move
            //Invoke("delayOpen", 2f); //5秒后调用 delayOpen () 函数  ，只调用一次  do not need to goto hero_ax again
        }

        if (toolstype == 0)
        {
            m_animator.SetBool("ham", false);
            m_animator.SetBool("axe", false);
            m_animator.SetBool("wood", false);
            m_animator.SetBool("stone", false);
            m_animator.SetBool("Run_ham", false);
            m_animator.SetBool("Run_ax", false);
            m_animator.SetBool("Run_wood", false);
            m_animator.SetBool("Run_stone", false);
            m_animator.SetBool("Attack_stop", false);
            m_animator.SetInteger("Tool_type", 0);
        }
    }

    //捡起
    private void OnTriggerExit2D(Collider2D collision)
    {
        //捡起斧头
        // UnityEngine.Debug.Log("持续碰撞:"+collision.gameObject.tag);
        toolstype = GetComponent<PickupSystem>().type;
        if (collision.gameObject.tag == "axe" && toolstype==1)
        {
            Debug.Log("axe");
            m_animator.SetBool("axe", true);
            m_animator.SetBool("Attack_stop", true);
            m_animator.SetInteger("Tool_type", 1);
            m_animator.SetBool("ham", false);
            m_animator.SetBool("wood", false);
            m_animator.SetBool("stone", false);
        }
        //捡起锤子
        if (collision.gameObject.tag == "pickaxe" && toolstype == 2)
        {
            Debug.Log("pickaxe");
            m_animator.SetBool("ham", true);
            m_animator.SetBool("Attack_stop", true);
            m_animator.SetInteger("Tool_type", 2);
            m_animator.SetBool("axe", false);
            m_animator.SetBool("wood", false);
            m_animator.SetBool("stone", false);
        }

        //捡起木头
        if (collision.gameObject.tag == "wood" && toolstype == 3)
        {
            Debug.Log("wood");
            m_animator.SetBool("wood", true);
            m_animator.SetInteger("Tool_type", 3);
            m_animator.SetBool("axe", false);
            m_animator.SetBool("ham", false);
            m_animator.SetBool("stone", false);
        }

        //捡起石头
        if (collision.gameObject.tag == "rock" && toolstype == 4)
        {
            Debug.Log("stone");
            m_animator.SetBool("stone", true);
            m_animator.SetInteger("Tool_type", 4);
            m_animator.SetBool("axe", false);
            m_animator.SetBool("ham", false);
            m_animator.SetBool("wood", false);
        }

        //丢下工具
        // if (toolstype == 0 && !(collision.gameObject.tag == "pickaxe") && !(collision.gameObject.tag == "axe"))
        if (toolstype==0)
        {
            m_animator.SetBool("ham", false);
            m_animator.SetBool("axe", false);
            m_animator.SetBool("wood", false);
            m_animator.SetBool("stone", false);
            m_animator.SetBool("Run_ham", false);
            m_animator.SetBool("Run_ax", false);
            m_animator.SetBool("Run_wood", false);
            m_animator.SetBool("Run_stone", false);
            m_animator.SetBool("Attack_stop", false);
            m_animator.SetInteger("Tool_type", 0);

        }
    }

    void delayOpen()
    {
        m_animator.SetBool("Attack_stop", true);
        m_animator.SetBool("axe", true);
    }

}
