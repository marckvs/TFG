using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum COMMAND
{
    none = 0, jump = 1, turnRight = 2, turnLeft = 3, checkPoint = 4, walk = 5, function = 6
}
[System.Serializable]
public class Command : MonoBehaviour
{
    public COMMAND command;
    public int commandPosition;
}
