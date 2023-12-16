using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListButton : MonoBehaviour
{
    public GameObject Game0;
    public GameObject Game1;
    public GameObject Game2;
    public GameObject Game3;
    public GameObject Game4;
    public GameObject Left;
    public GameObject Right;

    private int page = 0; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void next_page()
    {
        if(page == 0)
        {
            Game0.SetActive(false);
            Game1.transform.position = new Vector2(Screen.width * 410/1920, Screen.height * 540/1080);
            Game2.transform.position = new Vector2(Screen.width * 960/1920, Screen.height * 540/1080);
            Game3.SetActive(true);
            Left.SetActive(true);
            page = 1;
        }
        else if(page == 1)
        {
            Game1.SetActive(false);
            Game2.transform.position = new Vector2(Screen.width * 410/1920, Screen.height * 540/1080);
            Game3.transform.position = new Vector2(Screen.width * 960/1920, Screen.height * 540/1080);
            Game4.SetActive(true);
            Right.SetActive(false);
            page = 2;
        }
    }

    public void prev_page()
    {
        if(page == 1)
        {
            Game0.SetActive(true);
            Game1.transform.position = new Vector2(Screen.width * 960/1920, Screen.height * 540/1080);
            Game2.transform.position = new Vector2(Screen.width * 1510/1920, Screen.height * 540/1080);
            Game3.SetActive(false);
            Left.SetActive(false);
            page = 0;
        }
        else if(page == 2)
        {
            Game1.SetActive(true);
            Game2.transform.position = new Vector2(Screen.width * 960/1920, Screen.height * 540/1080);
            Game3.transform.position = new Vector2(Screen.width * 1510/1920, Screen.height * 540/1080);
            Game4.SetActive(false);
            Right.SetActive(true);
            page = 1;
        }
    }

}
