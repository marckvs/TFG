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

    private PlayerController playerController;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        cells = FindObjectsOfType<Cell>();
        SetCells();
        GameManager.I.RestartLevel(this);
    }

    public IEnumerator RunProgram()
    {
        buildSequenceOfcommands();
        StartCoroutine(CheckLevelFailed());

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
                        if (commandFunc != COMMAND.none)
                            commandsToExecute.Add(commandFunc);
                    }
                }
                if(commandsToExecute[i] != COMMAND.function)
                    yield return new WaitForSeconds(.5f);
            }

            SpawnPlayer();
            UIController.I.switchAllButtons(true);
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
        yield return new WaitForSeconds(2f);
        playerController.isLevelFailed = false;
        GameManager.I.RestartLevel(this);
    }

    public void LevelFailed()
    { 
        Debug.Log("LevelFailed");
    }

    public void LevelCompleted()
    {
        UIController.I.ShowNextLevelButton();
        Debug.Log("levelCompleted");
    }

    public void SpawnPlayer()
    {
        playerController.gameObject.transform.position = new Vector3(initialCell.cellPosX, initialCell.cellPosY, 0f);
        playerController.actualCell = initialCell;

        playerController.SetPlayerInitialTransition();
    }

    public void RestartLevelManager()
    {
        commandsToExecute = new List<COMMAND>();
        checkpointsChecked = 0;
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

        for (int i = 0; i < programCommands.Length; i++)
        {
            COMMAND commandPr = programCommands[i].GetComponent<Command>().command;
            if (commandPr != COMMAND.function)
            {
                if (commandPr != COMMAND.none) 
                    commandsToExecute.Add(commandPr);
            }
            else if (commandPr == COMMAND.function)
            {
                for (int j = 0; j < functionCommands.Length; j++)
                {
                    COMMAND commandFunc = functionCommands[j].GetComponent<Command>().command;
                    if (commandFunc != COMMAND.none)
                        commandsToExecute.Add(commandFunc);
                }
            }
        }

        for (int i = 0; i < commandsToExecute.Count; i++)
        {
            Debug.Log(commandsToExecute[i].ToString() + ", pos = " + i.ToString());
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
                checkPoint();
                break;
          
        }
    }
}
