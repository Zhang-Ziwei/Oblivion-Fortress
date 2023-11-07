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

    public GameObject tempicon_axe;
    public GameObject tempicon_pickaxe;
    public GameObject tempicon_wood;
    public GameObject tempicon_rock;

    public string Tag = "Base";
    public float depositRange = 3;
    bool locking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && (type == 1 || type == 2) && (!locking)) //put down tool
        {
            if (type == 1) //put down axe
            {
                Instantiate(axe, transform.position, Quaternion.identity, parent);
                tempicon_axe.SetActive(false);
            }
            else if (type == 2) //put down axe
            {
                Instantiate(pickaxe, transform.position, Quaternion.identity, parent);
                tempicon_pickaxe.SetActive(false);
            }
            type = 0;
            locking = true;
        }

        else if (Input.GetKeyDown("e") && (type == 3 || type == 4) && (!locking)) //put down material
        {
            GameObject nearestBase = GameData.getNearestObjectWithTag(transform.position, Tag);

            if(nearestBase && GameData.distanceRec(transform.position, nearestBase.transform.position) < depositRange) //if base exist
            {
                if (type == 3) //add 1 wood to base
                {
                    nearestBase.GetComponent<Base>().depositWood(1);
                    tempicon_wood.SetActive(false);
                }
                else if (type == 4) //add 1 rock to base
                {
                    nearestBase.GetComponent<Base>().depositStone(1);
                    tempicon_rock.SetActive(false);
                }
            }
            else //if base not exist
            {
                if (type == 3) //put down wood
                {
                    Instantiate(wood, transform.position, Quaternion.identity, parent);
                    tempicon_wood.SetActive(false);
                }
                else if (type == 4) //put down rock
                {
                    Instantiate(rock, transform.position, Quaternion.identity, parent);
                    tempicon_rock.SetActive(false);
                }
            }
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
        if (Input.GetKey("e") && type == 0 && !(locking)) //pick up
        {
            if (collision.gameObject.tag == "axe") //pick up axe
            {
                type = 1;
                tempicon_axe.SetActive(true);
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "pickaxe") //pick up pickaxe
            {
                type = 2;
                tempicon_pickaxe.SetActive(true);
                Destroy(collision.gameObject);
            }

            else if (collision.gameObject.tag == "wood") //pick up wood
            {
                type = 3;
                tempicon_wood.SetActive(true);
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "rock") //pick up rock
            {
                type = 4;
                tempicon_rock.SetActive(true);
                Destroy(collision.gameObject);
            }
            locking = true;
        }
    }

}
