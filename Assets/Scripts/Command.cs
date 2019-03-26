using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum COMMAND
{
    none = 0, jump = 1, turnLeft = 2, turnRight = 3, checkPoint = 4, move = 5
}
[System.Serializable]
public class Command : MonoBehaviour
{
    public COMMAND command;
    public int commandPosition;
}
