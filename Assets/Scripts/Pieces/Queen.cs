using System;
using UnityEngine;

public class Queen : BasePiece
{
    public Queen()
    {
        level = 8;
        baseLevel = 8;
        evolveLevel = 10;

        movement = new Vector3Int(7, 7, 7);
    }
    
    public override void Evolve()
    {
        base.Evolve();
        moveTwice = true;
    }
    
    public override string GetDescription()
    {
        string description = base.GetDescription();

        if (evolved)
        {
            description = $"<b><color=#76428a>Evolved upgrade</color></b>{Environment.NewLine}Can move twice in one turn";
        }
        else
        {
            description += Environment.NewLine + "<align=left>Once it has evolved it can move one twice in one turn";
        }

        return description;
    }
}