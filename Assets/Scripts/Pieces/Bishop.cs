using UnityEngine;

public class Bishop : BasePiece
{
    public Bishop()
    {
        level = 5;
        evolveLevel = 7;

        movement = new Vector3Int(0, 0, 7);
    }

    public override void Evolve()
    {
        base.Evolve();

        movement = new Vector3Int(1, 0, 7);
    }
}