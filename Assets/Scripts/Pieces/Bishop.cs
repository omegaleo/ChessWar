using UnityEngine;

public class Bishop : BasePiece
{
    public Bishop()
    {
        level = 5;
        evolveLevel = 7;

        movement = new Vector3Int(0, 0, 7);
    }
}