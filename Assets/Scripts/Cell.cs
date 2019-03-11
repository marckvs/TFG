using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour{

	public float cellPosX;
    public float cellPosY;
    public bool isCheckpoint;

    public List<Transition> TransitionsFromCell;

    public Transition transitionRight;
    public Transition transitionLeft;
    public Transition transitionUp;
    public Transition transitionDown;

    void Start()
    {
        cellPosX = this.gameObject.transform.position.x;
        cellPosY = this.gameObject.transform.position.y;

        TransitionsFromCell.Add(transitionRight);
        TransitionsFromCell.Add(transitionDown);
        TransitionsFromCell.Add(transitionLeft);
        TransitionsFromCell.Add(transitionUp);
    }

}
