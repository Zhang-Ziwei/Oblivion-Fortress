using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public int type = 0;
    /*
    0:empty
    1:axe
    2:pickaxe
    3:wood
    4:rock
    */
    public GameObject axe;
    public GameObject pickaxe;
    public GameObject wood;
    public GameObject rock;

    public Transform parent;

    public GameObject tempicon_wood;
    public GameObject tempicon_rock;

    public string Tag = "Base";
    public float depositRange = 3;
    bool locking = false;
    bool touchitem = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && type != 0 && !locking && !touchitem) //put down
        {
            putdown(type);
            type = 0;
            locking = true;
        }

        if (Input.GetKeyUp("e"))
        {
            locking = false;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey("e") && !(locking)) //pick up
        {
            int temp = type; // currently holding

            if (collision.gameObject.tag == "axe" && !(collision.gameObject.tag == "pickaxe") && !(collision.gameObject.tag == "wood") && !(collision.gameObject.tag == "rock")) //pick up axe
            {
                type = 1;
                Destroy(collision.gameObject);
                locking = true;
                putdown(temp);
            }
            else if (collision.gameObject.tag == "pickaxe" && !(collision.gameObject.tag == "axe") && !(collision.gameObject.tag == "wood") && !(collision.gameObject.tag == "rock")) //pick up pickaxe
            {
                type = 2;
                Destroy(collision.gameObject);
                locking = true;
                putdown(temp);
            }

            else if (collision.gameObject.tag == "wood" && !(collision.gameObject.tag == "pickaxe") && !(collision.gameObject.tag == "axe") && !(collision.gameObject.tag == "rock")) //pick up wood
            {
                type = 3;
                tempicon_wood.SetActive(true);
                Destroy(collision.gameObject);
                locking = true;
                putdown(temp);
            }
            else if (collision.gameObject.tag == "rock" && !(collision.gameObject.tag == "pickaxe") && !(collision.gameObject.tag == "axe") && !(collision.gameObject.tag == "wood")) //pick up rock
            {
                type = 4;
                tempicon_rock.SetActive(true);
                Destroy(collision.gameObject);
                locking = true;
                putdown(temp);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "wood" || (collision.gameObject.tag == "pickaxe") || (collision.gameObject.tag == "axe") || (collision.gameObject.tag == "rock"))
        {
            touchitem = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "wood" || (collision.gameObject.tag == "pickaxe") || (collision.gameObject.tag == "axe") || (collision.gameObject.tag == "rock"))
        {
            touchitem = false;
        }
    }

    void putdown(int item)
    {
        if (item == 1) //put down axe
        {
            Instantiate(axe, transform.position, Quaternion.identity, parent);
        }
        else if (item == 2) //put down axe
        {
            Instantiate(pickaxe, transform.position, Quaternion.identity, parent);
        }
        else if (item == 3 || item == 4)
        {
            // find the base
            GameObject nearestBase = GameData.getNearestObjectWithTag(transform.position, Tag);

            if(nearestBase && GameData.distanceRec(transform.position, nearestBase.transform.position) < depositRange) //if base exist
            {
                if (item == 3) //add 1 wood to base
                {
                    if (nearestBase.GetComponent<Base>().depositWood(1) != 0) Instantiate(wood, transform.position, Quaternion.identity, parent);
                    tempicon_wood.SetActive(false);
                }
                else if (item == 4) //add 1 rock to base
                {
                    if (nearestBase.GetComponent<Base>().depositStone(1) != 0) Instantiate(rock, transform.position, Quaternion.identity, parent);
                    tempicon_rock.SetActive(false);
                }
            }
            else //if base not exist
            {
                if (item == 3) //put down wood
                {
                    Instantiate(wood, transform.position, Quaternion.identity, parent);
                    tempicon_wood.SetActive(false);
                }
                else if (item == 4) //put down rock
                {
                    Instantiate(rock, transform.position, Quaternion.identity, parent);
                    tempicon_rock.SetActive(false);
                }
            }
        }
    }
}
