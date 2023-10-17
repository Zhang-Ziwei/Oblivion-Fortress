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
    private int rush_cyclenum = 40;
    private int update_totaltime = 41;// ����С�ڳ�̵�ʱ�䣬��Ȼ�ж�ʱ���ʱ��һ��ʼ�Ͱ��ٶȽ�Ϊʮ��֮һ
    private int update_temptime = 0;
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
    }
    // ���һ�ξ���
    private void rush()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            update_temptime = update_totaltime;
            movespeed = movespeed * 10;
        }
        if (update_totaltime-update_temptime == rush_cyclenum)
        {
            movespeed = movespeed / 10;
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
}
