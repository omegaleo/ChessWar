using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool botGame;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void StartGame(bool botGame)
    {
        this.botGame = botGame;

        SceneManager.LoadScene(1);
        StartCoroutine(AwaitForGameStart());
    }

    IEnumerator AwaitForGameStart()
    {
        while (PieceManager.instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        PieceManager.instance.Setup();
    }
}
