using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameManager), typeof(UIController))]
[DisallowMultipleComponent]
public class Loader : Singleton<Loader>
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SceneManager.LoadScene(1);
    }
}