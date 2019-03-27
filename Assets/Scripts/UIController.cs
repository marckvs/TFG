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
    public Image checkpointImage;
    public Image jumpImage;

    public Image programSpot1;
    public Image programSpot2;
    public Image programSpot3;
    public Image programSpot4;
    public Image programSpot5;
    public Image programSpot6;

    public int programButtonPressed;
    public bool isProgramButtonPressed;

    [HideInInspector]
    public Image[] programSpots;
    [HideInInspector]
    public PlayerController playerController;

    private float timerDeleteCommand = 0;
    private float timeToDeleteCommand = 1;

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

        programButtonPressed = 0;
        isProgramButtonPressed = false;
    }

    #region MainMenuUI

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnNewGameButtonPressed()
    {
        MainMenuScreen.SetActive(false);
        LevelMenuScreen.SetActive(true);
    }

    #endregion

    #region LevelMenuUI
    public void OnSelectLevelButtonPressed(int buildIndex)
    {
        LevelMenuScreen.SetActive(false);
        InGameMenu.SetActive(true);
        SceneManager.LoadScene(buildIndex);
    }

    public void OnMenuBackButtonPressed()
    {
        MainMenuScreen.SetActive(true);
        LevelMenuScreen.SetActive(false);
    }

    #endregion

    #region InGameMenu

    public void OnRestartButtonPressed()
    {
        checkReferenceLevelManager();

        GameManager.I.RestartLevel(levelManager);
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

    public void OnMovementButtonPressed(GameObject gO)
    {
        checkReferenceLevelManager();

        if (levelManager.programSpotsUsed < programSpots.Length)
        {
            if (!isProgramButtonPressed || programButtonPressed == levelManager.programSpotsUsed - 1)
            {
                changeAlpha(1, programSpots[levelManager.programSpotsUsed]);
                programSpots[levelManager.programSpotsUsed].sprite = gO.GetComponent<Image>().sprite;
                programSpots[levelManager.programSpotsUsed].GetComponent<Command>().command = gO.GetComponent<Command>().command;
                levelManager.programSpotsUsed++;
            }
            else
            {
                for (int i = levelManager.programSpotsUsed-1; i > programButtonPressed ; i--)
                {
                    changeAlpha(1, programSpots[i+1]);
                    programSpots[i + 1].sprite = programSpots[i].sprite;
                    programSpots[i + 1].GetComponent<Command>().command = programSpots[i].GetComponent<Command>().command;
                }

                programSpots[programButtonPressed + 1].sprite = gO.GetComponent<Image>().sprite;;
                programSpots[programButtonPressed + 1].GetComponent<Command>().command = gO.GetComponent<Command>().command;
                changeAlpha(1, programSpots[programButtonPressed]);

                levelManager.programSpotsUsed++;

                isProgramButtonPressed = false;
            }
        }
    }

    #endregion

    #region MainProgram

    public void OnMainProgramButtonPressed(Command command)
    {
        if (command.command != COMMAND.none)
        {
            changeAlpha(1f, programSpots[programButtonPressed]);
            programButtonPressed = command.commandPosition;

            if (command.command != COMMAND.none)
            {
                //Debug.Log(programButtonPressed);
                //Debug.Log(levelManager.programSpotsUsed);
                changeAlpha(0.6f, programSpots[programButtonPressed]);
                isProgramButtonPressed = true;
            }
            else isProgramButtonPressed = false;
        }
    }

    public void OnMainProgramButtonHold(Button button)
    {
        if(button.GetComponentInParent<Command>().command != COMMAND.none)
        {
            Debug.Log("dentro");
            button.gameObject.SetActive(true);
        }
    }

    public void OnDeleteCommandPressed()
    {
        for (int i = programButtonPressed; i < levelManager.programSpotsUsed - 1; i++)
        {
            programSpots[i].sprite = programSpots[i+1].sprite;
            programSpots[i].GetComponent<Command>().command = programSpots[i+1].GetComponent<Command>().command;
        }

        programSpots[levelManager.programSpotsUsed-1].sprite = null;
        programSpots[levelManager.programSpotsUsed-1].GetComponent<Command>().command = COMMAND.none;
        changeAlpha(0, programSpots[levelManager.programSpotsUsed-1]);
        levelManager.programSpotsUsed--;
    }
    #endregion

    public void RestartUI()
    {
        for (int i = 0; i < programSpots.Length; i++)
        {
            changeAlpha(0, programSpots[i]);
            programSpots[i].GetComponent<Command>().command = COMMAND.none;
        }
    }

    private void changeAlpha(float alpha, Image image)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private void checkReferenceLevelManager()
    {
        if(levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }
    }
}
