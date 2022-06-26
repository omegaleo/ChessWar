using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : InstancedBehaviour<GameManager>
{
    public bool botGame;

    [SerializeField] private Texture2D cursorTexture;

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public Difficulty difficulty = Difficulty.Normal;
    
    protected override void Awake()
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

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
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
