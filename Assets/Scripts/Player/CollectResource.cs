using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectResource : MonoBehaviour
{
    float count = 0;
    public GameObject Tree;
    public GameObject Stone;
    public GameObject wood;
    public GameObject rock;
    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            if((Tree != null) && (GetComponent<PickupSystem>().type == 1)) //if collide tree and holding axe
            {
                count += Time.deltaTime;
                if(count > 2)
                {
                    // collection competed
                    count = 0;
                    Vector3 Pos = Tree.transform.position;
                    // generate wood
                    Instantiate(wood, new Vector3(Pos.x,Pos.y,0), Quaternion.identity, parent);
                    Destroy(Tree);
                }
            }
            else if(Stone != null && (GetComponent<PickupSystem>().type == 2)) //if collide stone and holding pickaxe
            {
                count += Time.deltaTime;
                if(count > 2)
                {
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
