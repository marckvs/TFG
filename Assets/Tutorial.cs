using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public Image background;
    public Button button;

    public Image[] images;

    public int indexActiveSprite;

    void Start()
    {
        indexActiveSprite = 0;
        images[indexActiveSprite].gameObject.SetActive(true);
    }

    void Update()
    {
        if (indexActiveSprite == images.Length)
        {
            background.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }
    }

    public void ClickHandler()
    {
        
        images[indexActiveSprite].gameObject.SetActive(false);
        indexActiveSprite++;
        if (indexActiveSprite < images.Length)
            images[indexActiveSprite].gameObject.SetActive(true);
        
    }
}
