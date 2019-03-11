using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : MonoBehaviour {
    [HideInInspector]
    public Transform trans;
    public Cell actualCell;
    public Transition actualTransition;
    public int idActualTransition;

    void Start()
    {
        trans = gameObject.GetComponent<Transform>();
        idActualTransition = 0;
        Debug.Log(idActualTransition);
        Debug.Log(actualCell.TransitionsFromCell.Length);
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
    }

    public void Move()
    {
        if (actualTransition.transition == TRANSITION.none) return;
        else
        {
            actualCell = actualTransition.cell;
            actualTransition = actualCell.transitionForward;
            trans.position = new Vector3(actualCell.cellPosX, actualCell.cellPosY, 0f);
        }
    }

    public void turnRight()
    {
        if (idActualTransition < 3) idActualTransition++;
        else
        {
            idActualTransition = 0;
        }
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
    }

    public void turnLeft()
    {
        if (idActualTransition > 0) idActualTransition--;
        else
        {
            idActualTransition = 3;
        }
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
    }

}
