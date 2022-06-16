using UnityEngine;

public class King : BasePiece
{
    public King()
    {
        movement = new Vector3Int(1, 1, 1);
    }
    
    protected override void CheckEvolved()
    {
        return;
    }

    public override void Kill()
    {
        base.Kill();

        PieceManager.instance.isKingAlive = false;
    }
}