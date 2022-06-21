using System.Linq;
using UnityEngine;

public class Pawn : BasePiece
{
    public int baseLevel = 1; // To be used for promotion level calculation

    private bool isFirstMove = true;
    
    public Pawn()
    {
        level = 1;
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

    public override void Move()
    {
        base.Move();

        isFirstMove = false;
        
        CheckForPromotion();
    }

    private bool MatchesState(int targetX, int targetY, CellState targetState)
    {
        CellState state = Board.instance.ValidateCell(targetX, targetY, this);

        if (state == targetState)
        {
            var cell = Board.instance.allCells[targetX, targetY];

            if (cell != null && cell.currentPiece != null)
            {
                var piece = cell.currentPiece;
                if (piece.GetType() == typeof(King))
                {
                    return false;
                }
            }
            
            highlightedCells.Add(Board.instance.allCells[targetX, targetY]);
            return true;
        }

        return false;
    }

    public override void CheckPathing()
    {
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

            if (MatchesState(currentX, currentY + movement.y, CellState.Free))
            {
                if (isFirstMove)
                {
                    MatchesState(currentX, currentY + (movement.y * 2), CellState.Free);
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
            PieceManager.instance.PromotePiece(this, CurrentCell, color);
        }
    }
}