using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour {

    public Cell[] cells;
    public int LevelId;

    public Cell initialCell;
    public Cell previousCellChecked;

    public int programSpotsUsed = 0;
    public int functionSpotsUsed = 0;
    public int loopSpotsUsed = 0;

    public int nCheckpoints = 0;
    public int checkpointsChecked = 0;

    public GameObject instructions;

    public LEVELCLASS levelClass;
    public LEVEL level;

    private Image[] programCommands;
    private Image[] subProgramCommands;

    private List<COMMAND> commandsToExecute;
    private List<Button> buttonsCommandsToExecute;

    private CharacterController characterController;
    private Vector3 initRotation;

    void Awake()
    {
        cells = FindObjectsOfType<Cell>();
        SetCells();

        Instantiate(GameManager.I.character, new Vector3(initialCell.cellPosX, initialCell.cellPosY, 0f), Quaternion.Euler(new Vector3(0, 140, 0)));
        characterController = FindObjectOfType<CharacterController>();
        initRotation = characterController.transform.rotation.eulerAngles;

        GameManager.I.RestartLevel(this);
    }

    public IEnumerator RunProgram()
    {
        buildSequenceOfcommands();
        StartCoroutine(CheckLevelFailed());
        
        if (programSpotsUsed > 0)
        {
            UIController.I.backButton.interactable = false;
            UIController.I.runButton.interactable = false;

            for (int i = 0; i < commandsToExecute.Count; i++)
            {
                commandToAction(commandsToExecute[i]);

                if (i == commandsToExecute.Count - 1 && commandsToExecute[commandsToExecute.Count - 1] == COMMAND.function)
                {
                    for (int j = 0; j < subProgramCommands.Length; j++)
                    {
                        COMMAND commandFunc = subProgramCommands[j].GetComponent<Command>().command;
                        Button buttonFunc = subProgramCommands[j].GetComponent<Button>();

                        if (commandFunc != COMMAND.none)
                        {
                            buttonsCommandsToExecute.Add(buttonFunc);
                            commandsToExecute.Add(commandFunc);
                        }
                    }
                }

                ColorBlock c = buttonsCommandsToExecute[i].colors;
                c.disabledColor = new Color(c.disabledColor.r, c.disabledColor.g, c.disabledColor.b, 1);
                buttonsCommandsToExecute[i].colors = c;

                if (commandsToExecute[i] != COMMAND.function)
                {
                    yield return new WaitForSeconds(GameManager.I.stepDuration);
                    characterController.resetAnimations();
                    if (commandsToExecute[i] == COMMAND.jump || commandsToExecute[i] == COMMAND.checkPoint)
                        yield return new WaitForSeconds(GameManager.I.stepDuration*2);

                    c.disabledColor = new Color(c.disabledColor.r, c.disabledColor.g, c.disabledColor.b, .5f);
                    buttonsCommandsToExecute[i].colors = c;
                }
                if (GameManager.I.levelCompleted)
                {
                    yield break;
                }

            }

            SpawnCharacter();
            UIController.I.resetCheckPointCell();
            UIController.I.switchAllButtons(true);

            UIController.I.backButton.interactable = true;
            UIController.I.runButton.interactable = true;

            StopCoroutine(CheckLevelFailed());
            StopCoroutine(GameManager.I.CheckNotRunningProgram());
            StartCoroutine(GameManager.I.CheckNotRunningProgram());
        }
    }

    public IEnumerator CheckLevelFailed()
    {
        while (!characterController.isLevelFailed)
        {
            yield return null;
        }
        LevelFailed();
        yield return new WaitForSeconds(GameManager.I.stepDuration);

        UIController.I.levelFailedImage.gameObject.SetActive(false);
        characterController.isLevelFailed = false;
        GameManager.I.RestartLevel(this);
    }

    public void LevelFailed()
    {
        UIController.I.levelFailedImage.gameObject.SetActive(true);
        Debug.Log("LevelFailed");
    }

    public void LevelCompleted()
    {
        UIController.I.ShowNextLevelButton();
        Debug.Log("levelCompleted");
    }

    public void SpawnCharacter()
    {
        characterController.actualCell = initialCell;
        characterController.SetPlayerInitialTransition();
        characterController.transform.rotation = Quaternion.Euler(initRotation);
        characterController.transform.position = new Vector3(initialCell.cellPosX, initialCell.cellPosY, GameManager.I.zCharacterDisplacement);
    }

    public void RestartLevelManager()
    {
        commandsToExecute = new List<COMMAND>();
        buttonsCommandsToExecute = new List<Button>();
        previousCellChecked = null;
        SpawnCharacter();
    }

    private void checkPoint()
    {
        if (characterController.actualCell.isCheckpoint)
        {
            if (previousCellChecked == null)
            {
                checkpointsChecked++;
            }
            else if (previousCellChecked != characterController.actualCell)
            {
                checkpointsChecked++;
            }
            StartCoroutine(delayToSetCheckpoint());
        }
        else
        {
            Debug.LogError("you missed checkpoint");
            characterController.isLevelFailed = true;
        }

        previousCellChecked = characterController.actualCell;

        if (checkpointsChecked == nCheckpoints)
        {
            GameManager.I.levelCompleted = true;
        }
    }

    private void SetCells()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].SetCell();
            if (cells[i].isCheckpoint)
            {
                nCheckpoints++;
            }
        }
    }

    private void buildSequenceOfcommands()
    {
        programCommands = UIController.I.programSpots;

        if(UIController.I.isFunctionLevel)
            subProgramCommands = UIController.I.functionSpots;
        if(UIController.I.isLoopLevel)
            subProgramCommands = UIController.I.loopSpots;

        if (commandsToExecute.Count != 0) commandsToExecute.Clear();
        if (buttonsCommandsToExecute.Count != 0) buttonsCommandsToExecute.Clear();

        for (int i = 0; i < programCommands.Length; i++)
        {
            COMMAND commandPr = programCommands[i].GetComponent<Command>().command;
            Button buttonPr = programCommands[i].GetComponent<Button>();
             
            if (commandPr != COMMAND.function)
            {
                if (commandPr != COMMAND.none)
                {
                    commandsToExecute.Add(commandPr);
                    buttonsCommandsToExecute.Add(buttonPr);
                }              
            }
            else if (commandPr == COMMAND.function)
            {
                for (int j = 0; j < subProgramCommands.Length; j++)
                {
                    COMMAND commandFunc = subProgramCommands[j].GetComponent<Command>().command;
                    Button buttonFunc = subProgramCommands[j].GetComponent<Button>();

                    if (commandFunc != COMMAND.none)
                    {
                        commandsToExecute.Add(commandFunc);
                        buttonsCommandsToExecute.Add(buttonFunc);
                    }
                }
            }
        }
    }

    private void commandToAction(COMMAND command)
    {
        switch (command)
        {
            case COMMAND.walk:
                characterController.Move(COMMAND.walk);
                break;
            case COMMAND.jump:
                characterController.Move(COMMAND.jump);
                break;
            case COMMAND.turnLeft:
                characterController.turnLeft();
                break;
            case COMMAND.turnRight:
                characterController.turnRight();
                break;
            case COMMAND.checkPoint:
                characterController.checkPoint();
                checkPoint();
                break;     
        }
    }

    public void waitStepDuration()
    {
        if(characterController.initialPosition != characterController.transform.position)
        {
            SpawnCharacter();
           
        }
    }

    public IEnumerator delayToSetCheckpoint()
    {
        yield return new WaitForSeconds(GameManager.I.stepDuration);
        UIController.I.setCheckPointCell(characterController.actualCell, levelClass);
    }
}
