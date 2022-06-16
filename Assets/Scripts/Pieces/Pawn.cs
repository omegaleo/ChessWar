using UnityEngine;

public class Pawn : BasePiece
{
    private int baseLevel = 1; // To be used for promotion level calculation
    public Pawn()
    {
        level = 1;
    }
}