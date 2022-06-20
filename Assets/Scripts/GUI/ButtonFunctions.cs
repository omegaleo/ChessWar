using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void StartGameBot()
    {
        GameManager.instance.StartGame(true);
    }
    
    public void StartGame()
    {
        GameManager.instance.StartGame(false);
    }

    public void OpenHowToPlay()
    {
        HowToPlayScreen.instance.Open();
    }
}
