using UnityEngine;

public class Pawn : BasePiece
{
    public Pawn()
    {
        level = 1;
    }
    
    protected override void CheckEvolved()
    {
        if (Mathf.CeilToInt(level) == 3)
        {
            base.CheckEvolved();
        }
    }
}