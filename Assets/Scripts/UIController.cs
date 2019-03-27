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

    public Image MainProgram;
    public Image FunctionProgram;

    public Image programSpot1;
    public Image programSpot2;
    public Image programSpot3;
    public Image programSpot4;
    public Image programSpot5;
    public Image programSpot6;

    public Image functionSpot1;
    public Image functionSpot2;
    public Image functionSpot3;
    public Image functionSpot4;
    public Image functionSpot5;


    public int programButtonPressed = 0;
    public bool isProgramButtonPressed = false;

    public bool mainProgramButtonPressed;
    public bool functionProgramButtonPressed;

    [HideInInspector]
    public Image[] programSpots;
    public Image[] functionSpots;
    [HideInInspector]
    public PlayerController playerController;

    private bool deleteActivated = false;

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

        functionSpots = new Image[5];
        functionSpots[0] = functionSpot1;
        functionSpots[1] = functionSpot2;
        functionSpots[2] = functionSpot3;
        functionSpots[3] = functionSpot4;
        functionSpots[4] = functionSpot5;
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

    public void OnMainProgramSelected()
    {
        Debug.Log("program");
        mainProgramButtonPressed = true;
        functionProgramButtonPressed = false;

        changeAlpha(0.6f, FunctionProgram);
        changeAlpha(1f, MainProgram);

        switchFunctionButtons(false);
        switchProgramButtons(true);

        if (deleteActivated)
        {
            resetDeleteButtons();
        }
    }

    public void OnFunctionProgramSelected()
    {
        Debug.Log("function");

        mainProgramButtonPressed = false;
        functionProgramButtonPressed = true;

        changeAlpha(0.6f, MainProgram);
        changeAlpha(1f, FunctionProgram);

        switchFunctionButtons(true);
        switchProgramButtons(false);

        if (deleteActivated)
        {
            resetDeleteButtons();
        }
    }

    public void OnRestartButtonPressed()
    {
        checkReferenceLevelManager();
        resetDeleteButtons();
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
        //if (deletePressed) resetDeleteButton();

        GameManager.I.runningProgram = true;
    }

    private void addMovementButtonToProgram(Image[] array, ref int spotsUsed, GameObject gO)
    {
        if (spotsUsed < array.Length)
        {
            if (!isProgramButtonPressed || programButtonPressed == spotsUsed - 1)
            {
                changeAlpha(1, array[spotsUsed]);
                array[spotsUsed].sprite = gO.GetComponent<Image>().sprite;
                array[spotsUsed].GetComponent<Command>().command = gO.GetComponent<Command>().command;

                spotsUsed++;
            }
            else
            {
                for (int i = spotsUsed - 1; i > programButtonPressed; i--)
                {
                    changeAlpha(1, array[i + 1]);
                    array[i + 1].sprite = array[i].sprite;
                    array[i + 1].GetComponent<Command>().command = array[i].GetComponent<Command>().command;
                }

                array[programButtonPressed + 1].sprite = gO.GetComponent<Image>().sprite; ;
                array[programButtonPressed + 1].GetComponent<Command>().command = gO.GetComponent<Command>().command;
                changeAlpha(1, array[programButtonPressed]);

                spotsUsed++;

                isProgramButtonPressed = false;
            }
        }
    }

    public void OnMovementButtonPressed(GameObject gO)
    {
        if (deleteActivated) {
            resetDeleteButtons();
            return;
        }

        checkReferenceLevelManager();

        if (mainProgramButtonPressed)
        {
            addMovementButtonToProgram(programSpots, ref levelManager.programSpotsUsed, gO);
        }

        else if (functionProgramButtonPressed)
        {
            addMovementButtonToProgram(functionSpots, ref levelManager.functionSpotsUsed, gO);
        }
    }

    #endregion

    #region MainProgram

    public void OnMainProgramButtonPressed(Command command)
    {
        if (deleteActivated)
        {
            resetDeleteButtons();
            return;
        }

        if (command.command != COMMAND.none)
        {
            changeAlpha(1f, programSpots[programButtonPressed]);
            programButtonPressed = command.commandPosition;

            if (command.command != COMMAND.none)
            {
                changeAlpha(0.6f, programSpots[programButtonPressed]);
                isProgramButtonPressed = true;
            }
            else isProgramButtonPressed = false;
        }
    }

    public void OnMainProgramButtonHold(Button button)
    {

        if (button.GetComponentInParent<Command>().command != COMMAND.none)
        {
            programButtonPressed = button.GetComponentInParent<Command>().commandPosition;
            Debug.Log("dentro");
            deleteActivated = true;
            button.gameObject.SetActive(true);
        }
    }

    public void OnDeleteCommandPressed()
    {
        Debug.Log(programButtonPressed);
        if (mainProgramButtonPressed)
        {
            deleteCommand(programSpots, ref levelManager.programSpotsUsed);
        }

        else if (functionProgramButtonPressed)
        {
            deleteCommand(functionSpots, ref levelManager.functionSpotsUsed);
        }

        if (deleteActivated) resetDeleteButtons();
    }

    private void deleteCommand(Image[] array, ref int spotsUsed)
    {
        for (int i = programButtonPressed; i < spotsUsed - 1; i++)
        {
            array[i].sprite = array[i + 1].sprite;
            array[i].GetComponent<Command>().command = array[i + 1].GetComponent<Command>().command;
        }

        array[spotsUsed - 1].sprite = null;
        array[spotsUsed - 1].GetComponent<Command>().command = COMMAND.none;
        changeAlpha(0, array[spotsUsed - 1]);
        spotsUsed--;
    }
    #endregion

    public void RestartUI()
    {
        restartProgramUI(programSpots);
        restartProgramUI(functionSpots);

        if (deleteActivated)
        {
            resetDeleteButtons();
            return;
        }
    }

    private void restartProgramUI(Image[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            changeAlpha(0, array[i]);
            array[i].GetComponent<Command>().command = COMMAND.none;
        }
    }

    private void changeAlpha(float alpha, Image image)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private void resetDeleteButtons()
    {

        resetDeleteProgramButtons(programSpots);

        resetDeleteProgramButtons(functionSpots);

        deleteActivated = false;
    }

    private void resetDeleteProgramButtons(Image[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void checkReferenceLevelManager()
    {
        if(levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }
    }

    private void switchProgramButtons(bool b)
    {
        for (int i = 0; i < programSpots.Length; i++)
        {
            programSpots[i].GetComponent<Button>().interactable = b;
        }
    }

    private void switchFunctionButtons(bool b)
    {
        for (int i = 0; i < functionSpots.Length; i++)
        {
            functionSpots[i].GetComponent<Button>().interactable = b;
        }
    }
}
