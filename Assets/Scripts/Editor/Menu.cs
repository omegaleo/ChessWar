using UnityEditor;

public class Menu
{
    [MenuItem("Game Menu/Start Game (2-player)")]
    public static void StartGame()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.StartGame(false);
        }
    }
    
    [MenuItem("Game Menu/Start Game (vs AI)")]
    public static void StartGameAI()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.StartGame(true);
        }
    }
    
    [MenuItem("Game Menu/Restart")]
    public static void RestartGame()
    {
        if (PieceManager.instance != null)
        {
            PieceManager.instance.ResetGame();
        }
    }
}