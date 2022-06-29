using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class King : BasePiece
{
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
        return piece.GetType() != typeof(King) && piece.color != color;
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

    internal override void Move()
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

    public void TrySwitchRook()
    {
        if (IsChecked())
        {
            var rooks = PieceManager.instance.GetRooks(color);

            if (rooks.Any())
            {
                var rook = GetClosestRook(rooks);

                if (rook != null)
                {
                    var cell = CurrentCell;
                    var rookCell = rook.CurrentCell;

                    cell.currentPiece = rook;
                    rookCell.currentPiece = this;

                    CurrentCell = rookCell;
                    rook.CurrentCell = cell;
                    
                    transform.position = CurrentCell.transform.position;
                    rook.transform.position = rook.CurrentCell.transform.position;
                    
                    SFXManager.instance.Play("pieceMoveSFX");
                    SFXManager.instance.Play("pieceMoveSFX");

                    CurrentCell.checkedImage.enabled = false;
                    rook.CurrentCell.checkedImage.enabled = false;
                    
                    PieceManager.instance.UpdatePaths();
                    IsChecked();
                }
            }
        }
    }

    public override string GetDescription()
    {
        string description = "";

        if (evolved)
        {
            description = $"<b><color=#76428a>Evolved upgrade</color></b>{Environment.NewLine}Switches with the nearest rook when checked.";
        }
        else
        {
            description = $"{Environment.NewLine}Evolves when there are at least three evolved pieces.{Environment.NewLine}{Environment.NewLine}<align=left>Once it has evolved it will switch with the nearest rook when checked";
        }

        return description;
    }
    
    private BasePiece GetClosestRook(List<BasePiece> rooks)
    {
        var closestRook = rooks.FirstOrDefault();
        float currentDistance =
            Vector2Int.Distance(CurrentCell.boardPosition, closestRook.CurrentCell.boardPosition);

        foreach (var rook in rooks)
        {
            var distance = Vector2Int.Distance(CurrentCell.boardPosition, rook.CurrentCell.boardPosition);

            if (distance < currentDistance)
            {
                closestRook = rook;
            }
        }

        return closestRook;
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
                    if (rook.color != color || !rook.isFirstMove)
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