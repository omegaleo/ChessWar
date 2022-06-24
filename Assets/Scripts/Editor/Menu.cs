using UnityEditor;

/// <summary>
/// Class that handles the Main menu items for the game in the Editor
/// </summary>
public class Menu
{
    [MenuItem("Game Menu/Restart")]
    public static void RestartGame()
    {
        if (PieceManager.instance != null)
        {
            PieceManager.instance.ResetGame();
        }
    }
}