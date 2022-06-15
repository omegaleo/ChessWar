using UnityEngine;

public class Queen : BasePiece
{
    public Queen()
    {
        level = 8;
    }
    
    protected override void CheckEvolved()
    {
        if (Mathf.CeilToInt(level) == 10)
        {
            base.CheckEvolved();
        }
    }
}