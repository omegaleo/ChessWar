using UnityEngine;

public class Rook : BasePiece
{
    public Rook()
    {
        level = 4;
    }
    
    protected override void CheckEvolved()
    {
        if (Mathf.CeilToInt(level) == 6)
        {
            base.CheckEvolved();
        }
    }
}