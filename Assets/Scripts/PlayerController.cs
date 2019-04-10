using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class PlayerController : MonoBehaviour {
    [HideInInspector]

    public Animator animator;
    public Transform tr;
    public Cell actualCell;
    public Transition actualTransition;
    public int idActualTransition;
    public bool isLevelFailed = false;

    void Awake()
    {
        tr = gameObject.GetComponent<Transform>();
        animator = gameObject.GetComponent<Animator>();
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

            if (c == COMMAND.jump)
            {
                animator.SetBool("is_in_air", true);
                tr.DOJump(new Vector3(actualCell.cellPosX, actualCell.cellPosY, GameManager.I.zPlayerDisplacement), .6f, 1, GameManager.I.stepDuration).SetEase(Ease.InSine);
            }
            else
            {
                animator.SetBool("run", true);
                tr.DOMove(new Vector3(actualCell.cellPosX, actualCell.cellPosY, GameManager.I.zPlayerDisplacement), GameManager.I.stepDuration)/*.SetEase(Ease.InOutQuint)*/;
            }
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
        tr.DORotate(new Vector3(tr.rotation.eulerAngles.x, tr.rotation.eulerAngles.y + 90, tr.rotation.eulerAngles.z), GameManager.I.stepDuration).SetEase(Ease.OutQuint);

    }

    public void turnLeft()
    {
        if (idActualTransition > 0) idActualTransition--;
        else
        {
            idActualTransition = actualCell.TransitionsFromCell.Length - 1;
        }
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
        tr.DORotate(new Vector3(tr.rotation.eulerAngles.x, tr.rotation.eulerAngles.y - 90, tr.rotation.eulerAngles.z), GameManager.I.stepDuration).SetEase(Ease.OutQuint);

    }

    public void checkPoint()
    {
        animator.SetBool("checkpoint", true);
        
    }

    public void SetPlayerInitialTransition()
    {
        idActualTransition = (int) TRANSITIONDIRECTION.forward;
        actualTransition = actualCell.TransitionsFromCell[idActualTransition];
    }

    public void resetAnimations()
    {
        animator.SetBool("checkpoint", false);
        animator.SetBool("run", false);
        animator.SetBool("is_in_air", false);
    }
}
