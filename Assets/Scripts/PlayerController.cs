using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : MonoBehaviour {
    [HideInInspector]


    public Transform tr;
    public Cell actualCell;
    public Transition actualTransition;
    public int idActualTransition;
    public bool isLevelFailed = false;

    void Start()
    {
        tr = gameObject.GetComponent<Transform>();
    }

    public void Move(COMMAND c)
    {
        if (actualTransition.transition == TRANSITION.none)
        {
            Debug.LogError("you missed move");
            isLevelFailed = true;
        }
        else if ((actualTransition.transition == TRANSITION.jump && c == COMMAND.walk) ||
            (actualTransition.transition == TRANSITION.walk && c == COMMAND.jump))
        {
            Debug.LogError("you missed move");
            isLevelFailed = true;

        }
        else
        {
            actualCell = actualTransition.cell;
            actualTransition = actualCell.TransitionsFromCell[idActualTransition];
            tr.position = new Vector3(actualCell.cellPosX, actualCell.cellPosY, 0f);
        }
    }

    public void turnRight()
    {
        Debug.Log(idActualTransition);
        if (idActualTransition < actualCell.TransitionsFromCell.Length -1) idActualTransition++;
        else
        {
            idActualTransition = (int)TRANSITIONDIRECTION.forward;
        }
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
    }

    public void turnLeft()
    {
        if (idActualTransition > 0) idActualTransition--;
        else
        {
            idActualTransition = actualCell.TransitionsFromCell.Length - 1;
        }
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
    }

    public void SetPlayerInitialTransition()
    {
        idActualTransition = (int) TRANSITIONDIRECTION.forward;
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
    }

}
