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

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
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
