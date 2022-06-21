using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotionChoice : MonoBehaviour
{
    [SerializeField] private Image image;

    private string piece = "";
    private Pawn pawn;
    private Cell cell;

    public void SetPromotionPiece(Pawn pawn, Cell cell, Color teamColor, PieceSprite sprite, string piece, bool isEvolved)
    {
        this.piece = piece;
        this.pawn = pawn;
        this.cell = cell;
        image.sprite = (teamColor == Color.black)
            ? ((isEvolved) ? sprite.eBlackSprite : sprite.blackSprite)
            : ((isEvolved) ? sprite.eWhiteSprite : sprite.whiteSprite);
        
    }

    public void Promote()
    {
        PieceManager.instance.PromotePiece(pawn, cell, pawn.color, piece);
        PawnPromotionScreen.instance.Close();
    }
}
