using UnityEngine;

public class Bishop : BasePiece
{
    public Bishop()
    {
        level = 5;
    }
    
    protected override void CheckEvolved()
    {
        if (Mathf.CeilToInt(level) == 7)
        {
            base.CheckEvolved();
        }
    }
}