using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour {

    public Cell[] cells;
    public int LevelId;

    public Cell initialCell;

    public int programSpotsUsed;

    private Image[] programCommands;
    private PlayerController playerController;

    void Awake()
    {
        programSpotsUsed = 0;
        playerController = FindObjectOfType<PlayerController>();
        cells = FindObjectsOfType<Cell>();
        SetCells();
        GameManager.I.RestartLevel(this);
    }

    public IEnumerator RunProgram()
    {
        programCommands = UIController.I.programSpots;
        Debug.Log(programCommands.Length);
        if (programSpotsUsed > 0)
        {
            for (int i = 0; i < programCommands.Length; i++)
            {
                commandToAction(programCommands[i].GetComponent<Command>().command);
                yield return new WaitForSeconds(.5f);
            }

            Debug.Log("runProgram");
            SpawnPlayer();
            StopCoroutine(GameManager.I.CheckNotRunningProgram());
            StartCoroutine(GameManager.I.CheckNotRunningProgram());
        }

    }

    public void LevelCompleted()
    {

    }

    public void SpawnPlayer()
    {
        playerController.gameObject.transform.position = new Vector3(initialCell.cellPosX, initialCell.cellPosY, 0f);
        playerController.actualCell = initialCell;

        playerController.SetPlayerInitialTransition();

    }

    private void SetCells()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].SetCell();
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
           
            //TODO CHECKPOINT

        }
    }
}
