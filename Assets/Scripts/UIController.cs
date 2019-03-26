using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UIController : Singleton<UIController> {

    LevelManager levelManager;

    public GameObject MainMenuScreen;
    public GameObject LevelMenuScreen;
    public GameObject InGameMenu;

    public Image moveImage;
    public Image turnLeftImage;
    public Image turnRightImage;

    public Image programSpot1;
    public Image programSpot2;
    public Image programSpot3;
    public Image programSpot4;
    public Image programSpot5;
    public Image programSpot6;

    [HideInInspector]
    public Image[] programSpots;
    [HideInInspector]
    public PlayerController playerController;

    //TODO FIX CAPITAL LETTERS

    void Awake()
    {
        programSpots = new Image[6];
        programSpots[0] = programSpot1;
        programSpots[1] = programSpot2;
        programSpots[2] = programSpot3;
        programSpots[3] = programSpot4;
        programSpots[4] = programSpot5;
        programSpots[5] = programSpot6;
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

    public void OnLevelBackButtonPressed(int buildIndex)
    {
        InGameMenu.SetActive(false);
        LevelMenuScreen.SetActive(true);
        SceneManager.LoadScene(buildIndex);
    }

    public void OnRunLevelButtonPressed()
    {
        checkReferenceLevelManager();

        GameManager.I.runningProgram = true;
    }

    public void OnRestartButtonPressed()
    {
        checkReferenceLevelManager();

        GameManager.I.RestartLevel(levelManager);
    }

    public void OnClickButtonMainProgramm()
    {
        if (levelManager.programSpotsUsed > 0)
        {
            levelManager.programSpotsUsed--;
            changeAlpha(0, levelManager.programSpotsUsed);
        }
    }

    public void OnMenuBackButtonPressed()
    {
        MainMenuScreen.SetActive(true);
        LevelMenuScreen.SetActive(false);
    }

    public void onMovePressed()
    {
        checkReferenceLevelManager();

        if (levelManager.programSpotsUsed < programSpots.Length)
        {
            changeAlpha(1, levelManager.programSpotsUsed);
            programSpots[levelManager.programSpotsUsed].sprite = moveImage.sprite;
            programSpots[levelManager.programSpotsUsed].GetComponent<Command>().command = COMMAND.move;
            levelManager.programSpotsUsed++;
        }
    }

    public void onTurnLeftPressed()
    {
        checkReferenceLevelManager();

        if (levelManager.programSpotsUsed < programSpots.Length)
        {
            changeAlpha(1, levelManager.programSpotsUsed);
            programSpots[levelManager.programSpotsUsed].sprite = turnLeftImage.sprite;
            programSpots[levelManager.programSpotsUsed].GetComponent<Command>().command = COMMAND.turnLeft;
            levelManager.programSpotsUsed++;
        }
    }

    public void onTurnRightPressed()
    {
        checkReferenceLevelManager();

        if (levelManager.programSpotsUsed < programSpots.Length)
        {
            changeAlpha(1, levelManager.programSpotsUsed);
            programSpots[levelManager.programSpotsUsed].sprite = turnRightImage.sprite;
            programSpots[levelManager.programSpotsUsed].GetComponent<Command>().command = COMMAND.turnRight;
            levelManager.programSpotsUsed++;
        }
    }

    public void OnCheckPointPressed()
    {

    }

    public void RestartUI()
    {
        for (int i = 0; i < programSpots.Length; i++)
        {
            changeAlpha(0, i);
            programSpots[i].GetComponent<Command>().command = COMMAND.none;
        }
    }

    private void changeAlpha(int alpha, int position)
    {
        Color color = programSpots[position].color;
        color.a = alpha;
        programSpots[position].color = color;
    }

    private void checkReferenceLevelManager()
    {
        if(levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }
    }
}
