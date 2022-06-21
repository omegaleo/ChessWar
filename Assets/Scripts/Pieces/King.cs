using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class King : BasePiece
{
    private bool isFirstMove;

    private Rook leftRook, rightRook;

    public King()
    {
        movement = new Vector3Int(1, 1, 1);
        isFirstMove = true;
    }
    
    public override void CheckEvolved()
    {
        return;
    }

    public override void Kill(bool promotion = false)
    {
        base.Kill(promotion);

        PieceManager.instance.isKingAlive = false;
    }

    public override void CheckPathing()
    {
        base.CheckPathing();

        var opposingPossibleMovements = PieceManager.instance.GetAllOpposingPossibleMovements(color);

        highlightedCells = highlightedCells.Where(x => !opposingPossibleMovements.Contains(x)).ToList();

        if (isFirstMove)
        {
            rightRook = GetRook(1, 3);
            leftRook = GetRook(-1, 4);
        }

        if (IsChecked())
        {
            var checkingCells = PieceManager.instance.GetCheckingCells(color).ToList();

            highlightedCells = highlightedCells.Where(x => !checkingCells.Contains(x)).ToList();
        }
    }

    public override bool IsValidMovement(BasePiece piece)
    {
        return piece == null;
    }

    public bool IsChecked()
    {
        var checkingCells = PieceManager.instance.GetCheckingCells(color).Where(x => x.currentPiece != null && x.currentPiece.color != color);
        bool isChecked = checkingCells.Any();

        CurrentCell.checkedImage.enabled = isChecked;
        
        return isChecked;
    }
    
    public bool IsCheckMate()
    {
        CheckPathing();

        return IsChecked() && !highlightedCells.Any() && !PieceManager.instance.CanCheckmateBePrevented(color);
    }

    public override void Reset()
    {
        base.Reset();

        isFirstMove = true;
        leftRook = null;
        rightRook = null;
    }

    public override void Move()
    {
        CurrentCell.checkedImage.enabled = false;
        base.Move();
        isFirstMove = false;
        
        if (CanCastle(leftRook))
        {
            leftRook.Castle();
        }

        if (CanCastle(rightRook))
        {
            rightRook.Castle();
        }
    }

    private bool CanCastle(Rook rook)
    {
        return rook != null && rook.castleTriggerCell == CurrentCell;
    }

    private Rook GetRook(int direction, int count)
    {
        int currentX = CurrentCell.boardPosition.x;
        int currentY = CurrentCell.boardPosition.y;

        for (int i = 1; i < count; i++)
        {
            int offsetX = currentX + (i * direction);

            var state = Board.instance.ValidateCell(offsetX, currentY, this);

            if (state != CellState.Free)
            {
                return null;
            }
        }

        try
        {
            Cell rookCell = Board.instance.allCells[currentX + (count * direction), currentY];
            if (rookCell.currentPiece != null)
            {
                Rook rook = null;

                if (rookCell.currentPiece is Rook)
                {
                    rook = (Rook) rookCell.currentPiece;
                }

                if (rook != null)
                {
                    if (rook.color != color || !rook.firstMove)
                    {
                        return null;
                    }
            
                    highlightedCells.Add(rook.castleTriggerCell);
                    return rook;
                }
                else
                {
                    return null;
                }
            }
        }
        catch
        {
        }

        return null;
    }
}