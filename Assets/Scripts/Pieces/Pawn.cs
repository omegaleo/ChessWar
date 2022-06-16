using UnityEngine;

public class Pawn : BasePiece
{
    private int baseLevel = 1; // To be used for promotion level calculation

    private bool isFirstMove = true;
    
    public Pawn()
    {
        level = 1;
    }

    public override void Setup(Color newColor, PieceSprite sprites)
    {
        base.Setup(newColor, sprites);
        movement = (color == Color.black) ? new Vector3Int(0, -1, -1) : new Vector3Int(0, 1, 1);
    }

    public override void Evolve()
    {
        base.Evolve();
        movement = new Vector3Int(1, 1, 1);
    }

    protected override void Move()
    {
        base.Move();

        isFirstMove = false;
    }

    private bool MatchesState(int targetX, int targetY, CellState targetState)
    {
        CellState state = CurrentCell.board.ValidateCell(targetX, targetY, this);

        if (state == targetState)
        {
            highlightedCells.Add(CurrentCell.board.allCells[targetX, targetY]);
            return true;
        }

        return false;
    }

    protected override void CheckPathing()
    {
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
        }
    }

    public override void Reset()
    {
        base.Reset();
        isFirstMove = true;
    }
}