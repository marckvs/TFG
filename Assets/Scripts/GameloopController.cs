using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameloopController : MonoBehaviour {

    public GameObject MainMenuScreen;
    public GameObject LevelMenuScreen;
    public GameObject InGameMenu;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

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

    public void OnBackButtonPressed(int buildIndex)
    {
        InGameMenu.SetActive(false);
        LevelMenuScreen.SetActive(true);
        SceneManager.LoadScene(buildIndex);
    }
}
