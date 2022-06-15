using UnityEngine;

public class Knight : BasePiece
{
    public Knight()
    {
        level = 6;
    }
    
    protected override void CheckEvolved()
    {
        if (Mathf.RoundToInt(level) == 9)
        {
            base.CheckEvolved();
        }
    }
}