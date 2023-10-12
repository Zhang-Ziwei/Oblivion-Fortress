using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResource : MonoBehaviour
{
    int count = 0;
    GameObject Tree;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("f")){
            if(Tree != null){
                count ++;
                if(count > 500){
                    Debug.Log("cut tree");
                    count = 0;
                    Destroy(Tree);
                }
            }
        }
        if(Input.GetKeyUp("f")){
            count = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name == "TreeCollider"){
            Tree = other.gameObject;
        }
    }
 
    void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.name == "TreeCollider"){
            Tree = null;
        }
    }
}
