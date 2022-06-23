using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    public void Rematch()
    {
        PieceManager.instance.ResetGame();
        CheckmateScreen.instance.Close();
    }

    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene(0);
    }

    public void OptionsMenuOpen()
    {
        OptionsMenu.instance.Open();
    }
    
    public void OptionsMenuClose()
    {
        OptionsMenu.instance.Close();
    }
}
