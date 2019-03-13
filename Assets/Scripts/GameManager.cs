using System.Collections;
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

        InitCoroutines();
    }

    private IEnumerator CheckNotRunningProgram()
    {
        while (!runningProgram)
        {
            yield return null;
        }
        currentLevel.StartCoroutine(currentLevel.RunProgram());
    }

    private IEnumerator CheckLevelCompleted()
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