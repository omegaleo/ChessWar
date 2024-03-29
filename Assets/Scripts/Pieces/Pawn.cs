﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : BasePiece
{
    public Pawn()
    {
        level = 1;
        baseLevel = 1;
    }

    public override void Setup(Color newColor, PieceSprite sprites)
    {
        base.Setup(newColor, sprites);

        movement = (color == PieceManager.instance.player2Color) ? new Vector3Int(0, -1, -1) : new Vector3Int(0, 1, 1);
    }

    public override void Evolve()
    {
        base.Evolve();
        movement = new Vector3Int(1, 1, 1);
    }

    protected override void ExecuteMovement(Cell cell)
    {
        isFirstMove = false;
        base.ExecuteMovement(cell);
        CheckForPromotion();
    }

    public override string GetDescription()
    {
        string description = base.GetDescription();

        if (evolved)
        {
            description = $"<b><color=#76428a>Evolved upgrade</color></b>{Environment.NewLine}Can move one square in any direction";
        }
        else
        {
            description += Environment.NewLine + "<align=left>Once it has evolved it can move one square in any direction";
        }

        return description;
    }
    
    private bool MatchesState(int targetX, int targetY, CellState targetState, bool cantCatch = false)
    {
        CellState state = Board.instance.ValidateCell(targetX, targetY, this);

        if (state == targetState)
        {
            var cell = Board.instance.allCells[targetX, targetY];

            if (cell != null && cell.currentPiece != null)
            {
                var piece = cell.currentPiece;
                if (!IsValidMovement(piece))
                {
                    return false;
                }

                if (cantCatch)
                {
                    return false;
                }
                
                if (ValidChecking(piece))
                {
                    isChecking = true;
                    CurrentCell.outlineImage.enabled = true;
                }
            }
            
            highlightedCells.Add(Board.instance.allCells[targetX, targetY]);
            
            return true;
        }

        return false;
    }

    public override void CheckPathing()
    {
        if (PieceManager.instance.EvolvedQueenSecondMove(color))
        {
            return;
        }
        
        highlightedCells.Clear();
        if (evolved)
        {
            base.CheckPathing();
        }
        else
        {
            int currentX = CurrentCell.boardPosition.x;
            int currentY = CurrentCell.boardPosition.y;
        
            // Top Left Enemy
            MatchesState(currentX - movement.z, currentY + movement.z, CellState.Enemy);
        
            // Top Right Enemy
            MatchesState(currentX + movement.z, currentY + movement.z, CellState.Enemy);
        
            // Top Left Ally
            MatchesState(currentX - movement.z, currentY + movement.z, CellState.Friendly);
        
            // Top Right Ally
            MatchesState(currentX + movement.z, currentY + movement.z, CellState.Friendly);

            if (MatchesState(currentX, currentY + movement.y, CellState.Free, true))
            {
                if (isFirstMove)
                {
                    MatchesState(currentX, currentY + (movement.y * 2), CellState.Free, true);
                }
            }
            
            if (PieceManager.instance.GetKing(color).IsChecked() && this.GetType() != typeof(King))
            {
                var checkingCells = PieceManager.instance.GetCheckingCells(color);
                highlightedCells = highlightedCells.Where(x => checkingCells.Contains(x)).ToList();
            }
        }
    }

    public override void Reset()
    {
        base.Reset();
        isFirstMove = true;
    }

    private void CheckForPromotion()
    {
        int currentX = CurrentCell.boardPosition.x;
        int currentY = CurrentCell.boardPosition.y;

        CellState state = Board.instance.ValidateCell(currentX, currentY + movement.y, this);

        if (state == CellState.OutOfBounds)
        {
            if ((!GameManager.instance.botGame) || (GameManager.instance.botGame && color == PieceManager.instance.player1Color))
            {
                PawnPromotionScreen.instance.Open(this, CurrentCell);
            }
            else
            {
                PieceManager.instance.PromotePiece(this, CurrentCell, color, new List<string>()
                {
                    "Q",
                    "R",
                    "KN",
                    "B"
                }.Random());
            }
        }
    }

    public int GetAdditionalLevels()
    {
        return level - baseLevel;
    }
}