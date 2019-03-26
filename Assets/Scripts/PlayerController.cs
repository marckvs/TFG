﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : MonoBehaviour {
    [HideInInspector]


    public Transform tr;
    public Cell actualCell;
    public Transition actualTransition;
    public int idActualTransition;

    void Start()
    {
        tr = gameObject.GetComponent<Transform>();
    }

    public void Move()
    {
        if (actualTransition.transition == TRANSITION.none) Debug.LogError("you missed move");
        else
        {
            actualCell = actualTransition.cell;
            actualTransition = actualCell.transitionForward;
            tr.position = new Vector3(actualCell.cellPosX, actualCell.cellPosY, 0f);
        }
    }

    public void turnRight()
    {
        if (idActualTransition < actualCell.TransitionsFromCell.Length) idActualTransition++;
        else
        {
            idActualTransition = 0;
        }
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
        if (actualTransition.transition == TRANSITION.none) Debug.LogError("you missed right");
    }

    public void turnLeft()
    {
        if (idActualTransition > 0) idActualTransition--;
        else
        {
            idActualTransition = actualCell.TransitionsFromCell.Length - 1;
        }
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
        if (actualTransition.transition == TRANSITION.none) Debug.LogError("you missed right");
    }

    public void SetPlayerInitialTransition()
    {
        idActualTransition = (int) TRANSITIONDIRECTION.forward;
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
    }

}