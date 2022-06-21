using UnityEngine;

public class Rook : BasePiece
{
    public Cell castleTriggerCell;
    public Cell castleCell;

    public bool firstMove;
    
    public Rook()
    {
        level = 4;
        baseLevel = 4;
        evolveLevel = 6;

        movement = new(7, 7, 0);
        firstMove = true;
    }

    public override void Evolve()
    {
        base.Evolve();

        bypassMovement = true;
    }

    public override void Reset()
    {
        base.Reset();
        firstMove = true;
    }

    public override void Move()
    {
        base.Move();
        firstMove = false;
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
}