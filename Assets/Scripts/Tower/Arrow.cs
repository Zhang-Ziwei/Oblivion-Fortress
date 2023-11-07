using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 speed = new Vector3(0, 0, 0);
    public void SetSpeed(Vector3 newSpeed){
        speed = newSpeed;
        transform.rotation = Quaternion.Euler(0, 0, (float) (Math.Atan2(newSpeed.y, newSpeed.x) * (180/Math.PI)));
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime;
    }
}
