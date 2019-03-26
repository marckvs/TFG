﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    public LevelManager currentLevel;

    public bool levelCompleted = false;
    public bool runningProgram = false;

    public void RestartLevel(LevelManager levelManager)
    {
        currentLevel = levelManager;
        levelCompleted = false;
        runningProgram = false;
        currentLevel.programSpotsUsed = 0;

        UIController.I.RestartUI();

        currentLevel.StopAllCoroutines();
        StopAllCoroutines();
        InitCoroutines();
        currentLevel.RestartLevelManager();
    }

    public IEnumerator CheckNotRunningProgram()
    {
        runningProgram = false;
        while (!runningProgram)
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
        currentLevel.LevelCompleted();
    }

    public void InitCoroutines()
    {
        StartCoroutine(CheckNotRunningProgram());
        StartCoroutine(CheckLevelCompleted());
    }
}