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

    public void runProgram()
    {
        programCommands = UIController.I.programSpots;
        for (int i = 0; i < programCommands.Length; i++)
        {
            Debug.Log(programCommands[i].color);
            StartCoroutine(checkMovement(i));
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
    IEnumerator checkMovement(int position)
    {
        yield return new WaitForSeconds(1f);
        commandToAction(programCommands[position].GetComponent<Command>().command);
    }

}
