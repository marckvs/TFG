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

    [HideInInspector]
    public Transition[] TransitionsFromCell;

    void Awake()
    {
        cellPosX = this.gameObject.transform.position.x;
        cellPosY = this.gameObject.transform.position.y;

        TransitionsFromCell = new Transition[3];

        TransitionsFromCell[0] = transitionForward;
        TransitionsFromCell[1] = transitionUpwards;
        TransitionsFromCell[2] = transitionDownwards;
    }

}
