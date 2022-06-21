using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PieceLevelAssociation
{
    public string name;
    public int baseLevel;
    public int evolveLevel;
}

public class PawnPromotionScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private PromotionChoice queen;
    [SerializeField] private PromotionChoice rook;
    [SerializeField] private PromotionChoice knight;
    [SerializeField] private PromotionChoice bishop;

    public static PawnPromotionScreen instance;

    [SerializeField] private List<PieceLevelAssociation> levels = new List<PieceLevelAssociation>()
    {
        new PieceLevelAssociation() {baseLevel = 4, evolveLevel = 6, name = "R"},
        new PieceLevelAssociation() {baseLevel = 5, evolveLevel = 7, name = "B"},
        new PieceLevelAssociation() {baseLevel = 6, evolveLevel = 9, name = "KN"},
        new PieceLevelAssociation() {baseLevel = 8, evolveLevel = 10, name = "Q"}
    };

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

        var Queen = levels.FirstOrDefault(x => x.name == "Q");
        var Rook = levels.FirstOrDefault(x => x.name == "R");
        var Bishop = levels.FirstOrDefault(x => x.name == "B");
        var Knight = levels.FirstOrDefault(x => x.name == "KN");
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
