using UnityEditor;
using UnityEngine;

/// <summary>
/// Main class to host the cheats part of the game's menu in the Editor
/// </summary>
public class Cheats
{
    [MenuItem("Game Menu/Cheats/Evolve Black")]
    public static void EvolveBlack()
    {
        if (PieceManager.instance != null)
        {
            PieceManager.instance.EvolvePieces(Color.black);
        }
    }

    [MenuItem("Game Menu/Cheats/Evolve White")]
    public static void EvolveWhite()
    {
        if (PieceManager.instance != null)
        {
            PieceManager.instance.EvolvePieces(Color.white);
        }
    }
}