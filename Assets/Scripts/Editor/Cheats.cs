using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class Cheats
    {
        [MenuItem("Cheats/Evolve Black")]
        public static void EvolveBlack()
        {
            if (PieceManager.instance != null)
            {
                PieceManager.instance.EvolvePieces(Color.black);
            }
        }

        [MenuItem("Cheats/Evolve White")]
        public static void EvolveWhite()
        {
            if (PieceManager.instance != null)
            {
                PieceManager.instance.EvolvePieces(Color.white);
            }
        }
    }
}