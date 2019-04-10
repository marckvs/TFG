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
        cellPosY = this.gameObject.transform.position.y + 0.45f;

        TransitionsFromCell = new Transition[4];

        TransitionsFromCell[0] = transitionForward;
        TransitionsFromCell[1] = transitionDownwards;
        TransitionsFromCell[2] = transitionBackwards;
        TransitionsFromCell[3] = transitionUpwards;
    }

}
