using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager instance;

    public bool isKingAlive = true;
    
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private List<BasePiece> whitePieces = new List<BasePiece>();
    [SerializeField] private List<BasePiece> blackPieces = new List<BasePiece>();

    private string[] pieceOrder = new string[16]
    {
        "P",
        "P",
        "P",
        "P",
        "P",
        "P",
        "P",
        "P",
        "R",
        "KN",
        "B",
        "K",
        "Q",
        "B",
        "KN",
        "R"
    };

    private Dictionary<string, Type> pieceLibrary = new Dictionary<string, Type>()
    {
        {"P", typeof(Pawn)},
        {"R", typeof(Rook)},
        {"KN", typeof(Knight)},
        {"B", typeof(Bishop)},
        {"K", typeof(King)},
        {"Q", typeof(Queen)}
    };

    [SerializeField] private List<PieceSprite> sprites = new List<PieceSprite>()
    {
        new PieceSprite() {pieceIdentifier = "P"},
        new PieceSprite() {pieceIdentifier = "R"},
        new PieceSprite() {pieceIdentifier = "KN"},
        new PieceSprite() {pieceIdentifier = "B"},
        new PieceSprite() {pieceIdentifier = "K"},
        new PieceSprite() {pieceIdentifier = "Q"},
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Setup(Board board)
    {
        whitePieces = CreatePieces(Color.white, board);
        blackPieces = CreatePieces(Color.black, board);
        
        PlacePieces(1, 0, whitePieces, board);
        PlacePieces(6, 7, blackPieces, board);

        SwitchSides(Color.black);
    }

    private List<BasePiece> CreatePieces(Color teamColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        for (int i = 0; i < pieceOrder.Length; i++)
        {
            GameObject newPieceObj = Instantiate(piecePrefab);
            newPieceObj.transform.SetParent(transform);

            newPieceObj.transform.localScale = new Vector3(1, 1, 1);
            newPieceObj.transform.localRotation = Quaternion.identity;

            string key = pieceOrder[i];
            Type pieceType = pieceLibrary[key];

            BasePiece newPiece = (BasePiece) newPieceObj.AddComponent(pieceType);
            newPieces.Add(newPiece);
            
            newPiece.Setup(teamColor, sprites.FirstOrDefault(x => x.pieceIdentifier == key));
        }

        return newPieces;
    }

    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePiece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            pieces[i].Place(board.allCells[i, pawnRow]);
            pieces[i + 8].Place(board.allCells[i, royaltyRow]);
        }
    }

    private void SetInteractive(List<BasePiece> pieces, bool value)
    {
        foreach (BasePiece piece in pieces)
        {
            piece.enabled = value;
        }
    }

    public void SwitchSides(Color color)
    {
        if (!isKingAlive)
        {
            ResetPieces();

            isKingAlive = true;

            color = Color.black;
        }

        bool isBlackTurn = color == Color.white;
        
        SetInteractive(whitePieces, !isBlackTurn);
        SetInteractive(blackPieces, isBlackTurn);
    }

    public void ResetPieces()
    {
        foreach (BasePiece piece in whitePieces)
        {
            piece.Reset();
        }
        
        foreach (BasePiece piece in blackPieces)
        {
            piece.Reset();
        }
    }
}