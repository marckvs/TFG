using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToggleSound : MonoBehaviour {

    Button soundButton;
    Image image;
    Image imageDisabled;
    bool interactable = true;

    void SetButton()
    {
        soundButton = GameObject.FindGameObjectWithTag("sound").GetComponent<Button>();
        image = soundButton.GetComponent<Image>();
        soundButton.interactable = interactable;
    }
    
    void Update()
    {
        SetButton();
    }

    public void ClickButtonSound()
    {
        bool audio = gameObject.GetComponent<AudioSource>().mute;
        gameObject.GetComponent<AudioSource>().mute = !audio;
        Debug.Log(audio);

        if(audio == true)
        {
            soundButton.interactable = true;
            interactable = true;
        }
        else
        {
            soundButton.interactable = false;
            interactable = false;
        }
    }

}
