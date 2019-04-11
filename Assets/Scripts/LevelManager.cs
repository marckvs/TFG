using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public LEVELCLASS levelClass;
    public LEVEL level;

    private Image[] programCommands;
    private Image[] functionCommands;

    private List<COMMAND> commandsToExecute;
    private List<Button> buttonsCommandsToExecute;

    private PlayerController playerController;
    private Vector3 initRotation;

    void Awake()
    {
        cells = FindObjectsOfType<Cell>();
        SetCells();
        Instantiate(GameManager.I.player, new Vector3(initialCell.cellPosX, initialCell.cellPosY, 0f), Quaternion.Euler(new Vector3(0, 140, 0)));
        playerController = FindObjectOfType<PlayerController>();
        initRotation = playerController.transform.rotation.eulerAngles;
        Debug.Log(initRotation);
        GameManager.I.RestartLevel(this);
    }

    public IEnumerator RunProgram()
    {
        buildSequenceOfcommands();
        StartCoroutine(CheckLevelFailed());
        UIController.I.restartButton.interactable = false;
        UIController.I.backButton.interactable = false;
        UIController.I.runButton.interactable = false;


        if (programSpotsUsed > 0)
        {
            for (int i = 0; i < commandsToExecute.Count; i++)
            {
                commandToAction(commandsToExecute[i]);
                if (i == commandsToExecute.Count - 1 && commandsToExecute[commandsToExecute.Count - 1] == COMMAND.function)
                {
                    for (int j = 0; j < functionCommands.Length; j++)
                    {
                        COMMAND commandFunc = functionCommands[j].GetComponent<Command>().command;
                        Button buttonFunc = functionCommands[j].GetComponent<Button>();

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
                    playerController.resetAnimations();
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

            SpawnPlayer();
            UIController.I.resetCheckPointCell();
            UIController.I.switchAllButtons(true);

            UIController.I.restartButton.interactable = true;
            UIController.I.backButton.interactable = true;
            UIController.I.runButton.interactable = true;

            StopCoroutine(CheckLevelFailed());
            StopCoroutine(GameManager.I.CheckNotRunningProgram());
            StartCoroutine(GameManager.I.CheckNotRunningProgram());         
        }
    }

    public IEnumerator CheckLevelFailed()
    {
        while (!playerController.isLevelFailed)
        {
            yield return null;
        }
        LevelFailed();
        yield return new WaitForSeconds(GameManager.I.stepDuration);

        UIController.I.levelFailedImage.gameObject.SetActive(false);
        playerController.isLevelFailed = false;
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

    public void SpawnPlayer()
    {
        playerController.transform.rotation = Quaternion.Euler(initRotation);
        playerController.transform.position = new Vector3(initialCell.cellPosX, initialCell.cellPosY, GameManager.I.zPlayerDisplacement);
        playerController.actualCell = initialCell;
        playerController.SetPlayerInitialTransition();
    }

    public void RestartLevelManager()
    {
        commandsToExecute = new List<COMMAND>();
        buttonsCommandsToExecute = new List<Button>();
        previousCellChecked = null;
        SpawnPlayer();
    }

    private void checkPoint()
    {
        if (playerController.actualCell.isCheckpoint)
        {
            if (previousCellChecked == null)
            {
                checkpointsChecked++;
            }
            else if (previousCellChecked != playerController.actualCell)
            {
                checkpointsChecked++;
            }
            StartCoroutine(delayToSetCheckpoint());
        }
        else
        {
            Debug.LogError("you missed checkpoint");
            playerController.isLevelFailed = true;
        }

        previousCellChecked = playerController.actualCell;

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
            functionCommands = UIController.I.functionSpots;
        if(UIController.I.isLoopLevel)
            functionCommands = UIController.I.loopSpots;

        if (commandsToExecute.Count != 0) commandsToExecute.Clear();
        if (buttonsCommandsToExecute.Count != 0) buttonsCommandsToExecute.Clear();

        for (int i = 0; i < programCommands.Length; i++)
        {
            COMMAND commandPr = programCommands[i].GetComponent<Command>().command;
            Button buttonPr = programCommands[i].GetComponent<Button>();
            Debug.Log(buttonPr.GetComponent<Command>().commandPosition);
             
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
                for (int j = 0; j < functionCommands.Length; j++)
                {
                    COMMAND commandFunc = functionCommands[j].GetComponent<Command>().command;
                    Button buttonFunc = functionCommands[j].GetComponent<Button>();

                    if (commandFunc != COMMAND.none)
                    {
                        commandsToExecute.Add(commandFunc);
                        buttonsCommandsToExecute.Add(buttonFunc);
                    }
                }
            }
        }

        for (int i = 0; i < commandsToExecute.Count; i++)
        {
            //Debug.Log(commandsToExecute[i].ToString() + ", pos = " + i.ToString());
        }
    }

    private void commandToAction(COMMAND command)
    {
        Debug.Log(command.ToString());
        switch (command)
        {
            case COMMAND.walk:
                playerController.Move(COMMAND.walk);
                break;
            case COMMAND.jump:
                playerController.Move(COMMAND.jump);
                break;
            case COMMAND.turnLeft:
                playerController.turnLeft();
                break;
            case COMMAND.turnRight:
                playerController.turnRight();
                break;
            case COMMAND.checkPoint:
                playerController.checkPoint();
                checkPoint();
                break;
          
        }
    }

    public IEnumerator delayToSetCheckpoint()
    {
        yield return new WaitForSeconds(GameManager.I.stepDuration);
        UIController.I.setCheckPointCell(playerController.actualCell, levelClass);
    }
}
