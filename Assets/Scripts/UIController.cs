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

    public GameObject Scroll;

    public GameObject DisabledButtonsMainProgram;
    public GameObject DisabledButtonsFunctionProgram;
    public GameObject DisabledButtonsLoopProgram;

    public GameObject FunctionBackImage;

    public Sprite ImageMainProgramLinearLevels;
    public Sprite ImageMainProgramFunctionLevels;
    public Sprite ImageMainProgramLoopLevels;

    public Sprite checkPointLinear;
    public Sprite checkPointFunction;
    public Sprite checkPointLoop;

    public Sprite checkPointLinearActivated;
    public Sprite checkPointFunctionActivated;
    public Sprite checkPointLoopActivated;

    public Sprite[] commandsLinearLevels;
    public Sprite[] commandsFunctionLevels;
    public Sprite[] commandsLoopLevels;
    
    public Image moveImage;
    public Image turnLeftImage;
    public Image turnRightImage;
    public Image checkpointImage;
    public Image jumpImage;
    public Image functionImage;

    public Image MainProgram;
    public Image FunctionProgram;
    public Image LoopProgram;

    public Image[] commands;    

    public Image functionSpot1;
    public Image functionSpot2;
    public Image functionSpot3;
    public Image functionSpot4;
    public Image functionSpot5;
    public Image functionSpot6;

    public Image loopSpot1;
    public Image loopSpot2;
    public Image loopSpot3;
    public Image loopSpot4;
    public Image loopSpot5;
    public Image loopSpot6;
    public Image loopSpot7;


    public Button nextLevelButton;
    public Image nextLevelImage;
    public Image levelFailedImage;

    public Button restartButton;
    public Button backButton;
    public Button runButton;

    public int programButtonPressed = 0;

    public bool isAnyButtonPressed = false;

    public bool isMainProgramButtonPressed;
    public bool isFunctionProgramButtonPressed;
    public bool isLoopProgramButtonPressed;

    public Image[] programSpots;
    [HideInInspector]
    public Image[] functionSpots;
    [HideInInspector]
    public Image[] loopSpots;
    [HideInInspector]
    public PlayerController playerController;

    public bool isDeleteActivated = false;
    public bool isFunctionLevel;
    public bool isLoopLevel;

    private int distanceBetweenLevelsUI = 1100;
    public int scrollVelocity;

    //TODO FIX CAPITAL LETTERS

    void Awake()
    {
        #region initArrays

        functionSpots = new Image[6];
        functionSpots[0] = functionSpot1;
        functionSpots[1] = functionSpot2;
        functionSpots[2] = functionSpot3;
        functionSpots[3] = functionSpot4;
        functionSpots[4] = functionSpot5;
        functionSpots[5] = functionSpot6;

        loopSpots = new Image[7];
        loopSpots[0] = loopSpot1;
        loopSpots[1] = loopSpot2;
        loopSpots[2] = loopSpot3;
        loopSpots[3] = loopSpot4;
        loopSpots[4] = loopSpot5;
        loopSpots[5] = loopSpot6;
        loopSpots[6] = loopSpot7;
        #endregion
    }

    #region MainMenuUI

    public void OnNextLevelsMenuButtonPressed(bool next)
    {
        Debug.Log("in");
        if(next)
            StartCoroutine(ScrollLevelMenu(true));
        else
            StartCoroutine(ScrollLevelMenu(false));
    }

    private IEnumerator ScrollLevelMenu(bool right)
    {
        float xPos = Scroll.transform.localPosition.x;
        while (true)
        {
            if (right)
            {
                if (Mathf.Abs(Scroll.transform.localPosition.x) > Mathf.Abs(xPos - distanceBetweenLevelsUI + scrollVelocity))
                {
                    yield break;
                }
                Scroll.transform.localPosition = new Vector3(Scroll.transform.localPosition.x - scrollVelocity, Scroll.transform.localPosition.y, Scroll.transform.localPosition.z);
            }
            else
            {
                if (Mathf.Abs(Scroll.transform.localPosition.x) < Mathf.Abs(xPos + distanceBetweenLevelsUI - scrollVelocity/2))
                {
                    yield break;
                }
                Scroll.transform.localPosition = new Vector3(Scroll.transform.localPosition.x + scrollVelocity, Scroll.transform.localPosition.y, Scroll.transform.localPosition.z);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnNewGameButtonPressed()
    {
        MainMenuScreen.SetActive(false);
        LevelMenuScreen.SetActive(true);
        Scroll.transform.localPosition = Vector3.zero;
    }

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

    #region SetProgramsInGameMenu

    public void ProgramController()
    {
        checkReferenceLevelManager();
        setCommands();

        if(levelManager.levelClass == LEVELCLASS.lineal)
        {
            MainProgram.sprite = ImageMainProgramLinearLevels;
            setImageCommands(commandsLinearLevels);
            FunctionProgram.gameObject.SetActive(false);
            LoopProgram.gameObject.SetActive(false);
            isFunctionLevel = false;
            isLoopLevel = false;
            FunctionBackImage.SetActive(false);
            Debug.Log("dentro");
        }

        else if (levelManager.levelClass == LEVELCLASS.function)
        {
            MainProgram.sprite = ImageMainProgramFunctionLevels;
            setImageCommands(commandsFunctionLevels);
            FunctionProgram.gameObject.SetActive(true);
            LoopProgram.gameObject.SetActive(false);
            isFunctionLevel = true;
            isLoopLevel = false;
            FunctionBackImage.SetActive(true);

        }

        if (levelManager.levelClass == LEVELCLASS.loop)
        {
            MainProgram.sprite = ImageMainProgramLoopLevels;
            setImageCommands(commandsLoopLevels);
            FunctionProgram.gameObject.SetActive(false);
            LoopProgram.gameObject.SetActive(true);
            isFunctionLevel = false;
            isLoopLevel = true;
            loopSpots[loopSpots.Length - 1].GetComponent<Command>().command = COMMAND.function;
            loopSpots[loopSpots.Length - 1].sprite = functionImage.sprite;
            loopSpots[loopSpots.Length-1].GetComponent<Button>().interactable = false;
            changeAlpha(1, loopSpots[loopSpots.Length - 1]);
            FunctionBackImage.SetActive(true);

        }

        OnMainProgramSelected();
    }

    public void OnMainProgramSelected()
    {
        isMainProgramButtonPressed = true;
        changeAlpha(1f, MainProgram);

        isAnyButtonPressed = false;

        changeAlpha(1, functionImage);
        functionImage.GetComponent<Button>().interactable = true;

        if (isFunctionLevel)
        {
            isFunctionProgramButtonPressed = false;
            changeAlpha(0.6f, FunctionProgram);

            changeArrayAlpha(functionSpots, levelManager.functionSpotsUsed, 1);

            switchProgramButtons(true, true);
            switchFunctionButtons(false, true);
        }

        if (isLoopLevel)
        {
            isLoopProgramButtonPressed = false;
            changeAlpha(0.6f, LoopProgram);

            changeArrayAlpha(loopSpots, levelManager.loopSpotsUsed, 1);

            switchProgramButtons(true, true);
            switchLoopButtons(false, true);
        }
        resetDeleteButtons();   
    }

    public void OnFunctionProgramSelected()
    {
        isAnyButtonPressed = false;

        isMainProgramButtonPressed = false;
        isFunctionProgramButtonPressed = true;

        changeAlpha(0.6f, MainProgram);
        changeAlpha(1f, FunctionProgram);

        changeArrayAlpha(programSpots, levelManager.programSpotsUsed, 1);

        switchFunctionButtons(true, true);
        switchProgramButtons(false, true);

        changeAlpha(0.6f, functionImage);
        functionImage.GetComponent<Button>().interactable = false;

        if (isDeleteActivated)
        {
            resetDeleteButtons();
        }
    }

    public void OnLoopProgramSelected()
    {
        isAnyButtonPressed = false;

        isMainProgramButtonPressed = false;
        isLoopProgramButtonPressed = true;

        changeAlpha(0.6f, MainProgram);
        changeAlpha(1f, LoopProgram);

        changeArrayAlpha(programSpots, levelManager.programSpotsUsed, 1);

        switchLoopButtons(true, true);
        switchProgramButtons(false, true);

        changeAlpha(0.6f, functionImage);
        functionImage.GetComponent<Button>().interactable = false;

        if (isDeleteActivated)
        {
            resetDeleteButtons();
        }
    }
    #endregion

    #region InGameUI

    public void ShowNextLevelButton()
    {
        nextLevelButton.gameObject.SetActive(true);
        nextLevelImage.gameObject.SetActive(true);
    }

    public void OnNextLevelButtonPressed()
    {
        nextLevelButton.gameObject.SetActive(false);
        nextLevelImage.gameObject.SetActive(false);
        SceneManager.LoadScene((int)levelManager.level + 1);
        checkReferenceLevelManager();
        GameManager.I.RestartLevel(levelManager);
    }
    public void OnRestartButtonPressed()
    {
        if (isDeleteActivated) resetDeleteButtons();

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
        if (isDeleteActivated) resetDeleteButtons();

        switchAllButtons(false);

        GameManager.I.runningProgram = true;
    }

    private void addMovementButtonToProgram(Image[] array, int spotsUsed, GameObject gO)
    {
        if (spotsUsed < array.Length)
        {
            if (!isAnyButtonPressed || programButtonPressed == spotsUsed - 1)
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

                isAnyButtonPressed = false;
            }
        }
    }

    public void OnMovementButtonPressed(GameObject gO)
    {
        if (isDeleteActivated) {
            resetDeleteButtons();
            return;
        }

        if (isMainProgramButtonPressed && levelManager.programSpotsUsed < programSpots.Length)
        {
            addMovementButtonToProgram(programSpots, levelManager.programSpotsUsed, gO);
            levelManager.programSpotsUsed++;
        }

        else if (isFunctionLevel && isFunctionProgramButtonPressed && levelManager.functionSpotsUsed < functionSpots.Length)
        {
            addMovementButtonToProgram(functionSpots, levelManager.functionSpotsUsed, gO);
            levelManager.functionSpotsUsed++;
        }

        else if(isLoopLevel && isLoopProgramButtonPressed && levelManager.loopSpotsUsed < loopSpots.Length - 1)
        {
            addMovementButtonToProgram(loopSpots, levelManager.loopSpotsUsed, gO);
            levelManager.loopSpotsUsed++;          
        }
        Debug.Log(levelManager.loopSpotsUsed);
    }

    #endregion

    #region MainProgramButtonsControl

    public void OnMainProgramButtonPressed(Command command)
    {
        if (isDeleteActivated)
        {
            resetDeleteButtons();
            return;
        }

        if (command.command != COMMAND.none)
        {
            programButtonPressed = command.commandPosition;

            if (command.command != COMMAND.none)
            {
                if (isMainProgramButtonPressed)
                {
                    changeArrayAlpha(programSpots, levelManager.programSpotsUsed, 1);
                    changeAlpha(0.6f, programSpots[programButtonPressed]);
                }
                else if (isFunctionProgramButtonPressed)
                {
                    changeArrayAlpha(functionSpots, levelManager.functionSpotsUsed, 1);
                    changeAlpha(0.6f, functionSpots[programButtonPressed]);
                }
                else if (isLoopProgramButtonPressed)
                {
                    changeArrayAlpha(loopSpots, levelManager.loopSpotsUsed, 1);
                    changeAlpha(0.6f, loopSpots[programButtonPressed]);
                }
                isAnyButtonPressed = true;
            }
            else isAnyButtonPressed = false;
        }
    }



    //OnProgramButtonHold ; se usa para todos los tipos de programa
    public void OnMainProgramButtonHold(Button button)
    {
        isAnyButtonPressed = false;
        resetAllCommandsAlpha();

        if (button.GetComponentInParent<Command>().command != COMMAND.none)
        {
            button.GetComponentInParent<Button>().interactable = false;
            programButtonPressed = button.GetComponentInParent<Command>().commandPosition;
            isDeleteActivated = true;
            button.gameObject.SetActive(true);
        }
    }

    public void OnDeleteCommandPressed()
    {
        if (isMainProgramButtonPressed)
        {
            deleteCommand(programSpots, levelManager.programSpotsUsed);
            levelManager.programSpotsUsed--;
        }

        else if (isFunctionProgramButtonPressed)
        {
            deleteCommand(functionSpots, levelManager.functionSpotsUsed);
            levelManager.functionSpotsUsed--;
        }

        else if (isLoopProgramButtonPressed)
        {
            if (levelManager.loopSpotsUsed < loopSpots.Length)
            {
                deleteCommand(loopSpots, levelManager.loopSpotsUsed);
                levelManager.loopSpotsUsed--;
            }
        }

        Debug.Log(levelManager.loopSpotsUsed);

        if (isDeleteActivated) resetDeleteButtons();
    }

    private void deleteCommand(Image[] array, int spotsUsed)
    {
        for (int i = programButtonPressed; i < spotsUsed - 1; i++)
        {
            array[i].sprite = array[i + 1].sprite;
            array[i].GetComponent<Command>().command = array[i + 1].GetComponent<Command>().command;
        }

        array[spotsUsed - 1].sprite = null;
        array[spotsUsed - 1].GetComponent<Command>().command = COMMAND.none;
        changeAlpha(0, array[spotsUsed - 1]);
    }
    #endregion

#region commandsController

    private void setCommands()
    {
        if(levelManager.level == LEVEL.level1)
        {
            controlVisibilityCommands(2, false);
        }
        else if(levelManager.level == LEVEL.level2)
        {
            controlVisibilityCommands(0, true);
            controlVisibilityCommands(4, false);
        }
        else if(levelManager.level == LEVEL.level3)
        {
            controlVisibilityCommands(0, true);
            controlVisibilityCommands(5, false);
        }
        else
        {
            controlVisibilityCommands(0, true);
        }
    }

    private void controlVisibilityCommands(int n, bool b)
    {
        for (int i = n; i < commands.Length; i++)
        {
            commands[i].gameObject.SetActive(b);
        }

    }
    #endregion

    public void setCheckPointCell(Cell c, LEVELCLASS lc)
    {
        if(lc == LEVELCLASS.function)
        {
            c.GetComponent<SpriteRenderer>().sprite = checkPointFunctionActivated;
        }
        if (lc == LEVELCLASS.lineal)
        {
            c.GetComponent<SpriteRenderer>().sprite = checkPointLinearActivated;
        }
        if (lc == LEVELCLASS.loop)
        {
            c.GetComponent<SpriteRenderer>().sprite = checkPointLoopActivated;
        }
    }

    public void resetCheckPointCell()
    {
        Cell[] cells = levelManager.cells;
        LEVELCLASS lc = levelManager.levelClass;
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].isCheckpoint)
            {
                if(lc == LEVELCLASS.lineal)
                {
                    cells[i].GetComponent<SpriteRenderer>().sprite = checkPointLinear;
                }
                if (lc == LEVELCLASS.function)
                {
                    cells[i].GetComponent<SpriteRenderer>().sprite = checkPointFunction;
                }
                if (lc == LEVELCLASS.loop)
                {
                    cells[i].GetComponent<SpriteRenderer>().sprite = checkPointLoop;
                }
            }
        }

        levelManager.checkpointsChecked = 0;

    }
    public void RestartUI()
    {
        restartProgramUI(programSpots, false);
        if(isFunctionLevel) restartProgramUI(functionSpots, false);
        if (isLoopLevel) restartProgramUI(loopSpots, true);

        backButton.interactable = true;
        restartButton.interactable = true;
        runButton.interactable = true;

        resetCheckPointCell();

        programButtonPressed = 0;

        if (isDeleteActivated)
        {
            resetDeleteButtons();
            return;
        }
    }

    private void restartProgramUI(Image[] array, bool isLoopProgram)
    {
        if (isLoopProgram)
        {
            for (int i = 0; i < array.Length-1; i++)
            {
                changeAlpha(0, array[i]);
                array[i].GetComponent<Command>().command = COMMAND.none;
            }
        }
        else
        {
            for (int i = 0; i < array.Length; i++)
            {
                changeAlpha(0, array[i]);
                array[i].GetComponent<Command>().command = COMMAND.none;
            }
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

        resetDeleteProgramButtons(programSpots, programSpots.Length);

        if(isFunctionLevel) resetDeleteProgramButtons(functionSpots, functionSpots.Length);
        if (isLoopLevel) resetDeleteProgramButtons(loopSpots, loopSpots.Length-1);

        isDeleteActivated = false;
    }

    private void resetDeleteProgramButtons(Image[] array, int size)
    {
        for (int i = 0; i < size; i++)
        {
            array[i].transform.GetChild(0).gameObject.SetActive(false);
            array[i].GetComponent<Button>().interactable = true;
        }
    }

    private void changeArrayAlpha(Image[] array, int sizeToChange, int alphavalue)
    {
        for (int i = 0; i < sizeToChange; i++)
        {
            changeAlpha(alphavalue, array[i]);
        }
    }

    private void resetAllCommandsAlpha()
    {
        changeArrayAlpha(programSpots, levelManager.programSpotsUsed, 1);
        changeArrayAlpha(functionSpots, levelManager.functionSpotsUsed, 1);
        changeArrayAlpha(loopSpots, levelManager.loopSpotsUsed, 1);
    }

    private void checkReferenceLevelManager()
    {
        if(levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }
    }

    private void switchProgramButtons(bool interactable, bool parent)
    {
        for (int i = 0; i < programSpots.Length; i++)
        {
            programSpots[i].GetComponent<Button>().interactable = interactable;
            if (parent) programSpots[i].transform.SetParent(DisabledButtonsMainProgram.transform);
            if (i < functionSpots.Length && parent)
            {
                if (isLoopLevel) loopSpots[i].transform.SetParent(LoopProgram.transform);
                if (isFunctionLevel) functionSpots[i].transform.SetParent(FunctionProgram.transform);
            }
        }
    }

    private void switchFunctionButtons(bool interactable, bool parent)
    {
        for (int i = 0; i < programSpots.Length; i++)
        {
            if (i < functionSpots.Length)
            {
                functionSpots[i].GetComponent<Button>().interactable = interactable;
                if (parent)functionSpots[i].transform.SetParent(DisabledButtonsFunctionProgram.transform);
            }
            programSpots[i].transform.SetParent(MainProgram.transform);
        }

    }

    private void switchLoopButtons(bool interactable, bool parent)
    {
        for (int i = 0; i < programSpots.Length - 1; i++)
        {
            if (i < loopSpots.Length)
            {
                if(i != loopSpots.Length - 1)
                    loopSpots[i].GetComponent<Button>().interactable = interactable;
               if(parent) loopSpots[i].transform.SetParent(DisabledButtonsLoopProgram.transform);
            }
            programSpots[i].transform.SetParent(MainProgram.transform);
        }
    }

    public void switchAllButtons(bool b)
    {
        switchFunctionButtons(b, false);
        switchLoopButtons(b, false);
        switchProgramButtons(b, false);
    }

    private void setImageCommands(Sprite[] s)
    {
        Debug.Log(commands.Length);
        for (int i = 0; i < commands.Length; i++)
        {
            commands[i].GetComponent<Image>().sprite = s[i];
        }
    }

    

}
