using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SettingButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Button RestartButton;
    public GameObject BuildingUI;
    public GameObject SmallBuildingUI;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame0()
    {
        SceneManager.LoadScene("Tutorial");
        Time.timeScale = 1;
    }

    public void RestartGame1()
    {
        SceneManager.LoadScene("Chapter1");
        Time.timeScale = 1;
    }

    public void RestartGame2()
    {
        SceneManager.LoadScene("Chapter2");
        Time.timeScale = 1;
    }

    public void RestartGame3()
    {
        SceneManager.LoadScene("Chapter3");
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void GoChapterList()
    {
        SceneManager.LoadScene("List");
        Time.timeScale = 1;
    }

    public void CloseBuildingUI()
    {
        BuildingUI.SetActive(false);
        SmallBuildingUI.SetActive(true);
    }

    public void OpenBuildingUI()
    {
        BuildingUI.SetActive(true);
        SmallBuildingUI.SetActive(false);
    }
}
