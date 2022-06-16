using UnityEngine;

public class Knight : BasePiece
{
    public Knight()
    {
        level = 6;
        evolveLevel = 9;
    }

    private void CreateCellPath(int flipper)
    {
        int currentX = CurrentCell.boardPosition.x;
        int currentY = CurrentCell.boardPosition.y;
        
        // Left
        MatchesState(currentX - 2, currentY + (1 * flipper));
        
        // Upper Left
        MatchesState(currentX - 1, currentY + (2 * flipper));
        
        // Upper Right
        MatchesState(currentX + 1, currentY + (2 * flipper));
        
        // Right
        MatchesState(currentX + 2, currentY + (1 * flipper));

        if (evolved)
        {
            // Horizontal Movement by 1
            MatchesState(currentX - 1, currentY);
            MatchesState(currentX + 1, currentY);
        }
    }

    protected override void CheckPathing()
    {
        highlightedCells.Clear();
        CreateCellPath(1);
        
        CreateCellPath(-1);
    }

    private void MatchesState(int targetX, int targetY)
    {
        CellState state = CurrentCell.board.ValidateCell(targetX, targetY, this);

        if (state == CellState.Friendly)
        {
            var possibleTargetCell = CurrentCell.board.allCells[targetX, targetY];

            var piece = possibleTargetCell.currentPiece;

            if (IsValidMovement(piece))
            {
                highlightedCells.Add(possibleTargetCell);
            }
        }
        else if (state != CellState.OutOfBounds)
        {
            highlightedCells.Add(CurrentCell.board.allCells [targetX, targetY]);
        }
    }
}