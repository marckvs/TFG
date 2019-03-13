﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRANSITION
{
    none = 0, jump = 1, walk = 2
}

[System.Serializable]
public class Transition {
    public TRANSITION transition;
    public Cell cell;
}
