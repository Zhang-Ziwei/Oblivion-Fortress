using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private int toolstype = 0;

    // walk and rush in playcontroller
    public float movespeed = 2f;
    public Camera maincamera;
    private Animation animate;
    private int rush_cyclenum = 100;
    private int update_totaltime = 101;// must more than rush_cyclenum
    private int update_temptime = 0;
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
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
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
                enemy.GetComponent<Enemy>().DeductHealth(Damage);
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
        toolstype = GetComponent<PickupSystem>().type;
        //Move(toolstype);
        rush(toolstype);

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Attack
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > AttackInterval && !m_rolling && toolstype == 0)
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
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > AttackInterval && !m_rolling && toolstype == 1)
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
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > AttackInterval && !m_rolling && toolstype == 2)
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
            m_animator.SetBool("Run_ham", false);
            m_animator.SetBool("Run_ax", false);
            m_animator.SetBool("Run_wood", false);
            m_animator.SetBool("Attack_stop", false);
            m_animator.SetInteger("Tool_type", 0);
        }

            /*
            // Block
            else if (Input.GetMouseButtonDown(1) && !m_rolling)
            {
                m_animator.SetTrigger("Block");
                m_animator.SetBool("IdleBlock", true);
            }

            else if (Input.GetMouseButtonUp(1))
                m_animator.SetBool("IdleBlock", false);
            */
    }

        // Animation Events
        // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    //捡起
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        //捡起斧头
        // UnityEngine.Debug.Log("持续碰撞:"+collision.gameObject.tag);
        toolstype = GetComponent<PickupSystem>().type;
        if (collision.gameObject.tag == "axe" && toolstype==1)
        {
            m_animator.SetBool("axe", true);
            m_animator.SetBool("Attack_stop", true);
            m_animator.SetInteger("Tool_type", 1);
            m_animator.SetBool("ham", false);
            m_animator.SetBool("wood", false);
            //toolstype = 1;
            //m_animator.SetTrigger("HeroKnight_ax");
            //toolstype = collision.GetComponent<PickUp>().toolstype;    // get the other other Collider2D involved in this collision's PickUp.cs toolstype parameter
            //UnityEngine.Debug.Log(toolstype);
        }
        //捡起锤子
        if (collision.gameObject.tag == "pickaxe" && toolstype == 2)
        {
            m_animator.SetBool("ham", true);
            m_animator.SetBool("Attack_stop", true);
            m_animator.SetInteger("Tool_type", 2);
            m_animator.SetBool("axe", false);
            m_animator.SetBool("wood", false);
            //toolstype = 2;
        }

        //捡起木头
        if (collision.gameObject.tag == "wood" && toolstype == 3)
        {
            m_animator.SetBool("wood", true);
            m_animator.SetInteger("Tool_type", 3);
            m_animator.SetBool("axe", false);
            m_animator.SetBool("ham", false);
            //toolstype = 2;
        }

        //丢下工具
        // if (toolstype == 0 && !(collision.gameObject.tag == "pickaxe") && !(collision.gameObject.tag == "axe"))
        if (toolstype==0)
        {
            m_animator.SetBool("ham", false);
            m_animator.SetBool("axe", false);
            m_animator.SetBool("wood", false);
            m_animator.SetBool("Run_ham", false);
            m_animator.SetBool("Run_ax", false);
            m_animator.SetBool("Run_wood", false);
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
