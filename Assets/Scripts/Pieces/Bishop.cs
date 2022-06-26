using System;
using UnityEngine;

public class Bishop : BasePiece
{
    public Bishop()
    {
        level = 5;
        baseLevel = 5;
        evolveLevel = 7;

        movement = new Vector3Int(0, 0, 7);
    }

    public override void Evolve()
    {
        base.Evolve();

        movement = new Vector3Int(1, 0, 7);
    }

    public override string GetDescription()
    {
        string description = base.GetDescription();

        if (evolved)
        {
            description = $"<b><color=#76428a>Evolved upgrade</color></b>{Environment.NewLine}Can move one square horizontally";
        }
        else
        {
            description += Environment.NewLine + "<align=left>Once it has evolved it can move one square horizontally";
        }

        return description;
    }
}