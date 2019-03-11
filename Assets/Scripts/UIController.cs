using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UIController : Singleton<UIController> {

    public GameObject MainMenuScreen;
    public GameObject LevelMenuScreen;
    public GameObject InGameMenu;

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnNewGameButtonPressed()
    {
        MainMenuScreen.SetActive(false);
        LevelMenuScreen.SetActive(true);
    }

    public void OnSelectLevelButtonPressed(int buildIndex)
    {
        LevelMenuScreen.SetActive(false);
        InGameMenu.SetActive(true);
        SceneManager.LoadScene(buildIndex);
    }

    public void OnLevelBackButtonPressed(int buildIndex)
    {
        InGameMenu.SetActive(false);
        LevelMenuScreen.SetActive(true);
        SceneManager.LoadScene(buildIndex);
    }

    public void OnMenuBackButtonPressed()
    {
        MainMenuScreen.SetActive(true);
        LevelMenuScreen.SetActive(false);
    }
}
