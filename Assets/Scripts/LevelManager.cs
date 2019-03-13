using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour {

    public List<Cell> cells;
    public int LevelId;

    public int programSpotsUsed;

    private Image[] programCommands;
    private PlayerController playerController;

    private void Awake()
    {
        programSpotsUsed = 0;
        GameManager.I.RestartLevel(this);
        playerController = FindObjectOfType<PlayerController>();
    }

    public IEnumerator RunProgram()
    {
        programCommands = UIController.I.programSpots;
        for (int i = 0; i < programCommands.Length; i++)
        {
            Debug.Log(programCommands[i].color);
            commandToAction(programCommands[i].GetComponent<Command>().command);
            yield return new WaitForSeconds(.5f);
        }

        GameManager.I.runningProgram = false;
    }

    public void LevelCompleted()
    {

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
