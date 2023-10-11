using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movetest : MonoBehaviour
{
    public float movespeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow)){
            transform.Translate(movespeed*Time.deltaTime,0,0);
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            transform.Translate(-movespeed*Time.deltaTime,0,0);
        }
        else if(Input.GetKey(KeyCode.UpArrow)){
            transform.Translate(0,movespeed*Time.deltaTime,0);
        }
        else if(Input.GetKey(KeyCode.DownArrow)){
            transform.Translate(0,-movespeed*Time.deltaTime,0);
        }
    }
}
