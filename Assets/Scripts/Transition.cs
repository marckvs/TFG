using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRANSITION
{
    walk = 0, jump = 1, none = 2
}

[System.Serializable]
public class Transition {
    public TRANSITION transition;
    public Cell cell;
}
