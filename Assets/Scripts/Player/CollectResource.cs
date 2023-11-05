using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectResource : MonoBehaviour
{
    float count = 0;
    public GameObject Tree;
    public GameObject Stone;
    public GameObject wooden;
    public GameObject rock;
    public int wood = 0;
    public int woodLimit = 2;
    public Transform parent;
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
                    // collection competed
                    wood = Math.Min(woodLimit, wood + 2);
                    count = 0;
                    Vector3 Pos = Tree.transform.position;
                    // generate wood
                    Instantiate(wooden, new Vector3(Pos.x,Pos.y,0), Quaternion.identity, parent);
                    Destroy(Tree);
                }
            }
            else if(Stone != null){
                count += Time.deltaTime;
                if(count > 2){
                    // collection competed
                    count = 0;
                    Vector3 Pos = Stone.transform.position;
                    // generate rock
                    Instantiate(rock, new Vector3(Pos.x,Pos.y,0), Quaternion.identity, parent);
                    Destroy(Stone);
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            // collection aborted
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
