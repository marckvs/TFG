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

    public int nCheckpoints = 0;
    public int checkpointsChecked = 0;

    private Image[] programCommands;
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
        programCommands = UIController.I.programSpots;

        if (programSpotsUsed > 0)
        {
            for (int i = 0; i < programCommands.Length; i++)
            {
                commandToAction(programCommands[i].GetComponent<Command>().command);
                yield return new WaitForSeconds(.5f);
            }

            SpawnPlayer();
            StopCoroutine(GameManager.I.CheckNotRunningProgram());
            StartCoroutine(GameManager.I.CheckNotRunningProgram());
        }

    }

    public void LevelCompleted()
    {
        Debug.Log("levelCompleted");
    }

    public void SpawnPlayer()
    {
        playerController.gameObject.transform.position = new Vector3(initialCell.cellPosX, initialCell.cellPosY, 0f);
        playerController.actualCell = initialCell;

        playerController.SetPlayerInitialTransition();

    }

    public void checkPoint()
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

        previousCellChecked = playerController.actualCell;

        if (checkpointsChecked == nCheckpoints)
        {
            GameManager.I.levelCompleted = true;
        }
    }

    public void RestartLevelManager()
    {
        checkpointsChecked = 0;
        SpawnPlayer();
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

    private void commandToAction(COMMAND command)
    {
        switch (command)
        {
            case COMMAND.move:
                playerController.Move();
                break;
            case COMMAND.jump:
                playerController.Move();
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
           
            //TODO CHECKPOINT

        }
    }
}
