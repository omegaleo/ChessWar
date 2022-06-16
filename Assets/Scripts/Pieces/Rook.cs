using UnityEngine;

public class Rook : BasePiece
{
    public Rook()
    {
        level = 4;
        evolveLevel = 6;

        movement = new(7, 7, 0);
    }
}