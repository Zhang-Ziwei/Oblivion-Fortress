using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float movespeed = 1f;
    public Camera maincamera;
    private Animation animate;
    private int rush_cyclenum = 50;
    private int update_totaltime = 51;// must more than rush_cyclenum
    private int update_temptime = 0;
    private bool PauseEnable = false;
    private bool isRush = false;
    public GameObject pauseUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        update_totaltime++;
        Move();
        rush();
        pausegame();
    }
    // ���һ�ξ���
    private void rush()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isRush==false)
        {
            update_temptime = update_totaltime;
            movespeed = movespeed * 5;
            isRush = true;
        }
        if (update_totaltime-update_temptime == rush_cyclenum)
        {
            movespeed = movespeed / 5;
        }
        if (update_totaltime - update_temptime == rush_cyclenum+20)
        {
            isRush=false;
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
        if(maincamera != null)
        {
            maincamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2.0f);
        }
    }

    void pausegame()
    {
        //pause menu
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(PauseEnable == false){
                PauseEnable = true;
                Time.timeScale = 0;
                pauseUI.SetActive(true);
            }
            else if(PauseEnable == true){
                PauseEnable = false;
                Time.timeScale = 1;
                pauseUI.SetActive(false);
            }
        }
    }
}
