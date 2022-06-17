﻿using System.Collections.Generic;
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
    
    protected override void CheckEvolved()
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

        rightRook = GetRook(1, 3);
        leftRook = GetRook(-1, 4);

        if (IsChecked())
        {
            var checkingCells = (color == Color.black)
                ? PieceManager.instance.whitePieces.Where(x => x.isChecking).Select(x => x.highlightedCells)
                : PieceManager.instance.blackPieces.Where(x => x.isChecking).Select(x => x.highlightedCells);

            highlightedCells = highlightedCells.Where(x => !checkingCells.Any(y => y.Contains(x))).ToList();
        }
    }

    public bool IsChecked()
    {
        bool isChecked = (color == Color.black)
            ? PieceManager.instance.whitePieces.Any(x => x.isChecking)
            : PieceManager.instance.blackPieces.Any(x => x.isChecking);

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

    protected override void Move()
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
        if (!isFirstMove)
        {
            return null;
        }

        int currentX = CurrentCell.boardPosition.x;
        int currentY = CurrentCell.boardPosition.y;

        for (int i = 1; i < count; i++)
        {
            int offsetX = currentX + (i * direction);

            var state = CurrentCell.board.ValidateCell(offsetX, currentY, this);

            if (state != CellState.Free)
            {
                return null;
            }
        }

        Cell rookCell = CurrentCell.board.allCells[currentX + (count * direction), currentY];
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
        
        
        
        return null;
    }
}