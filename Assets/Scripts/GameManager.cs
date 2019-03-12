using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    public LevelManager currentLevel;

    public void RestartLevel(LevelManager levelManager)
    {
        currentLevel = levelManager;
    }
}