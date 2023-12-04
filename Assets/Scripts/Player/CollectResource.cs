using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CollectResource : MonoBehaviour
{
    float count = 0;
    float CollectTime = 2; //the time need for collection
    public GameObject Tree;
    public GameObject Stone;
    public GameObject wood;
    public GameObject rock;
    public Transform parent;

    public Slider CollectBar;

    public AudioClip tree_sound;
    public AudioClip rock_sound;
    AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            if((Tree != null) && (GetComponent<PickupSystem>().type == 1)) //if collide tree and holding axe
            {
                CollectBar.gameObject.SetActive(true);
                CollectBar.value = count / CollectTime;
                count += Time.deltaTime;

                if(!audiosource.isPlaying)
                {
                    audiosource.PlayOneShot(tree_sound);
                }

                if(count > CollectTime)
                {
                    // collection competed
                    count = 0;
                    CollectBar.gameObject.SetActive(false);
                    CollectBar.value = 0;
                    Vector3 Pos = Tree.transform.position;
                    // generate wood
                    Instantiate(wood, new Vector3(Pos.x,Pos.y,0), Quaternion.identity, parent);
                    Destroy(Tree);
                }
            }
            else if(Stone != null && (GetComponent<PickupSystem>().type == 2)) //if collide stone and holding pickaxe
            {
                CollectBar.gameObject.SetActive(true);
                CollectBar.value = count / CollectTime;
                count += Time.deltaTime;

                if(!audiosource.isPlaying)
                {
                    audiosource.PlayOneShot(rock_sound);
                }

                if(count > CollectTime)
                {
                    // collection competed
                    count = 0;
                    CollectBar.gameObject.SetActive(false);
                    CollectBar.value = 0;
                    Vector3 Pos = Stone.transform.position;
                    // generate rock
                    Instantiate(rock, new Vector3(Pos.x,Pos.y,0), Quaternion.identity, parent);
                    Destroy(Stone);
                }
            }
            else
            {
                count = 0;
                audiosource.Stop();
                CollectBar.gameObject.SetActive(false);
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            // collection aborted
            count = 0;
            audiosource.Stop();
            CollectBar.gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "tree"){
            Tree = other.gameObject;
        }
        else if(other.gameObject.tag == "stone"){
            Stone = other.gameObject;
        }
    }
 
    void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "tree"){
            Tree = null;
        }
        else if(other.gameObject.tag == "stone"){
            Stone = null;
        }
    }
}
