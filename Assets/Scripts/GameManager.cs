﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LEVELCLASS
{
    lineal = 0, function = 1, loop = 2
}

public enum LEVEL
{
    level1 = 2, level2 = 3, level3 = 4, level4 = 5, level5 = 6, level6 = 7, level7 = 8, level8 = 9, level9 = 10, level10 = 11, level11 = 12
}

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    public LevelManager currentLevel;

    public bool levelCompleted = false;
    public bool runningProgram = false;

    public GameObject character;

    public float stepDuration;
    public float zCharacterDisplacement;

    public int numPassedLevels;
    public int numLevels = 11;

    public IEnumerator CheckNotRunningProgram()
    {
        yield return new WaitForSeconds(stepDuration); //When the game is restarted, waits until the last
        currentLevel.waitStepDuration();               //step of the character to avoid DOTween bugs.

        runningProgram = false;
        while (!runningProgram)//The RunProgram corotuine will not be started until runProgram is true.
        {
            yield return null;
        }
        currentLevel.StartCoroutine(currentLevel.RunProgram());
    }

    public IEnumerator CheckLevelCompleted()
    {
        while (!levelCompleted)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        currentLevel.LevelCompleted(); //Wait 1 second to show the screen to go to the next level
    }

    public void InitCoroutines()
    {
        StartCoroutine(CheckNotRunningProgram());
        StartCoroutine(CheckLevelCompleted());
    }

    public void RestartLevel(LevelManager levelManager)
    {
        currentLevel = levelManager;
        levelCompleted = false;
        runningProgram = false;
        currentLevel.programSpotsUsed = 0;
        currentLevel.functionSpotsUsed = 0;
        currentLevel.loopSpotsUsed = 0;

        UIController.I.ProgramController(); //Manages the UI of the game depending on the section
        UIController.I.RestartUI(); 

        currentLevel.StopAllCoroutines(); //Stops the RunProgram coroutine of the levelManager
        StopAllCoroutines(); //Stops the gameManager coroutines
        currentLevel.RestartLevelManager();
        InitCoroutines();
    }
}