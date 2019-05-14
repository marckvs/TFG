using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell : MonoBehaviour{

	public float cellPosX;
    public float cellPosY;
    public bool isCheckpoint;

    public Transition transitionForward;
    public Transition transitionDownwards;
    public Transition transitionUpwards;
    public Transition transitionBackwards;

    [HideInInspector]
    public Transition[] TransitionsFromCell;

    public void SetCell()
    {
        cellPosX = this.gameObject.transform.position.x;
        cellPosY = this.gameObject.transform.position.y + 0.45f; //Magic number to set the character position correctly

        TransitionsFromCell = new Transition[4];

        TransitionsFromCell[(int)TRANSITIONDIRECTION.forward] = transitionForward;
        TransitionsFromCell[(int)TRANSITIONDIRECTION.downwards] = transitionDownwards;
        TransitionsFromCell[(int)TRANSITIONDIRECTION.backwards] = transitionBackwards;
        TransitionsFromCell[(int)TRANSITIONDIRECTION.upwards] = transitionUpwards;
    }

}
