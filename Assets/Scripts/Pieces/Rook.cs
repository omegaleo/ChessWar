﻿using System;
using UnityEngine;

public class Rook : BasePiece
{
    public Cell castleTriggerCell;
    public Cell castleCell;

    public Rook()
    {
        level = 4;
        baseLevel = 4;
        evolveLevel = 6;

        movement = new(7, 7, 0);
        isFirstMove = true;
    }

    public override void Evolve()
    {
        base.Evolve();

        bypassMovement = true;
    }

    public override void Reset()
    {
        base.Reset();
        isFirstMove = true;
    }

    internal override void Move()
    {
        base.Move();
        isFirstMove = false;
    }

    public override void Place(Cell newCell)
    {
        base.Place(newCell);
        
        int triggerOffset = CurrentCell.boardPosition.x < 4 ? 2 : -1;
        castleTriggerCell = SetCell(triggerOffset);
        
        int castleOffset = CurrentCell.boardPosition.x < 4 ? 3 : -2;
        castleCell = SetCell(castleOffset);
    }

    public void Castle()
    {
        targetCell = castleCell;

        Move();
    }

    private Cell SetCell(int offset)
    {
        var newPosition = CurrentCell.boardPosition;
        newPosition.x += offset;

        return Board.instance.allCells[newPosition.x, newPosition.y];
    }
    
    public override string GetDescription()
    {
        string description = base.GetDescription();

        if (evolved)
        {
            description = $"<b><color=#76428a>Evolved upgrade</color></b>{Environment.NewLine}Can move over pieces of the same color";
        }
        else
        {
            description += Environment.NewLine + "<align=left>Once it has evolved it can move over pieces of the same color";
        }

        return description;
    }
}