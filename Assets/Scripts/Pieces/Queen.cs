using UnityEngine;

public class Queen : BasePiece
{
    public Queen()
    {
        level = 8;
        evolveLevel = 10;

        movement = new Vector3Int(7, 7, 7);
    }
}