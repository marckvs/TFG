using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRANSITION
{
    none = 0, jump = 1, walk = 2
}

public enum TRANSITIONDIRECTION
{
    forward = 0, downwards = 1, backwards = 2, upwards = 3
}

[System.Serializable]
public class Transition {
    public TRANSITION transition;
    public Cell cell;
}
