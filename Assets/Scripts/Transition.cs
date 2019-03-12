using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRANSITION
{
    walk = 2, jump = 1, none = 0
}

[System.Serializable]
public class Transition {
    public TRANSITION transition;
    public Cell cell;
}
