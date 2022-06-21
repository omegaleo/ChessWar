using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PawnPromotionScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private PromotionChoice queen;
    [SerializeField] private PromotionChoice rook;
    [SerializeField] private PromotionChoice knight;
    [SerializeField] private PromotionChoice bishop;

    public static PawnPromotionScreen instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Open(Pawn pawn, Cell cell)
    {
        panel.SetActive(true);
        
        queen.SetPromotionPiece(pawn, cell, pawn.color, PieceManager.instance.sprites.FirstOrDefault(x => x.pieceIdentifier == "Q"), "Q", Queen.baseLevel + pawn.GetAdditionalLevels() >= Queen.evolveLevel);
        rook.SetPromotionPiece(pawn, cell, pawn.color, PieceManager.instance.sprites.FirstOrDefault(x => x.pieceIdentifier == "R"), "R", Rook.baseLevel + pawn.GetAdditionalLevels() >= Rook.evolveLevel);
        knight.SetPromotionPiece(pawn, cell, pawn.color, PieceManager.instance.sprites.FirstOrDefault(x => x.pieceIdentifier == "KN"), "KN", Knight.baseLevel + pawn.GetAdditionalLevels() >= Knight.evolveLevel);
        bishop.SetPromotionPiece(pawn, cell, pawn.color, PieceManager.instance.sprites.FirstOrDefault(x => x.pieceIdentifier == "B"), "B", Bishop.baseLevel + pawn.GetAdditionalLevels() >= Bishop.evolveLevel);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
