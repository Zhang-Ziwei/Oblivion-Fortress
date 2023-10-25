using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectResource : MonoBehaviour
{
    float count = 0;
    GameObject Tree;
    GameObject Stone;
    public int wood = 0;
    public int woodLimit = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            if(Tree != null){
                count += Time.deltaTime;
                if(count > 2){
                    Debug.Log("remove tree");
                    wood = Math.Min(woodLimit, wood + 2);
                    count = 0;
                    Destroy(Tree);
                }
            }
            else if(Stone != null){
                count += Time.deltaTime;
                if(count > 2){
                    Debug.Log("remove stone");
                    count = 0;
                    Destroy(Stone);
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            count = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name == "TreeCollider"){
            Tree = other.gameObject;
        }
        else if(other.gameObject.name == "StoneCollider"){
            Stone = other.gameObject;
        }
    }
 
    void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.name == "TreeCollider"){
            Tree = null;
        }
        else if(other.gameObject.name == "StoneCollider"){
            Stone = null;
        }
    }
}
