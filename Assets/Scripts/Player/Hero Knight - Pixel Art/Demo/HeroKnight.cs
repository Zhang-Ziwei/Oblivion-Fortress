using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
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
    private string toolstype;

    // walk and rush in playcontroller
    public float movespeed = 1f;
    public Camera maincamera;
    private Animation animate;
    private int rush_cyclenum = 100;
    private int update_totaltime = 101;// must more than rush_cyclenum
    private int update_temptime = 0;
    private bool PauseEnable = false;
    private bool isRush = false;
    public GameObject pauseUI;

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
    private void rush()
    {
        rushTimer -= Time.deltaTime;
        rushCoolDownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && isRush == false)
        {
            //update_temptime = update_totaltime;
            rushTimer = rushTime / 3 * 2;
            rushCoolDownTimer = rushTime;
            movespeed = movespeed * 3;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            isRush = true;
            isSpeedUp = true;
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

    private void Move()
    {
        Vector3 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.D))
        {
            dir += new Vector3(movespeed * Time.deltaTime, 0, 0);
            transform.Translate(movespeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir += new Vector3(-movespeed * Time.deltaTime, 0, 0);
            transform.Translate(-movespeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            dir += new Vector3(0, movespeed * Time.deltaTime, 0);
            transform.Translate(0, movespeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += new Vector3(0, -movespeed * Time.deltaTime, 0);
            transform.Translate(0, -movespeed * Time.deltaTime, 0);
        }
        transform.position += dir;
    }

    private void LateUpdate()
    {
        if (maincamera != null)
        {
            maincamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2.0f);
        }
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

    // Update is called once per frame
    void Update ()
    {
        // walk and rush in playcontroller
        update_totaltime++;
        Move();
        rush();
        pausegame();
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

        // WHAT IS THIS CODE FUCKING DOING???
        //Death
        if (Input.GetKeyDown("o") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("p") && !m_rolling)
            m_animator.SetTrigger("Hurt");


        //Attack
        else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > AttackInterval && !m_rolling)
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

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);
        /*
        // Roll
        else if (Input.GetKeyDown("left ctrl") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
        */
        /*
        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
        */
        /*
        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        */
        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
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

    //捡起斧头
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        UnityEngine.Debug.Log("持续碰撞:");
        if (Input.GetKey(KeyCode.Space))
        {
            m_animator.SetBool("axe", true);
            m_animator.SetTrigger("HeroKnight_ax");
            toolstype = collision.GetComponent<PickUp>().toolstype;    // get the other other Collider2D involved in this collision's PickUp.cs toolstype parameter
            UnityEngine.Debug.Log(toolstype);
        }    
    }
    */
        
}
