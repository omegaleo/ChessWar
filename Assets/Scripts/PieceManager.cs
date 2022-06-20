using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PieceManager : MonoBehaviour
{
    public static PieceManager instance;

    public bool isKingAlive = true;
    
    [SerializeField] private GameObject piecePrefab;
    public List<BasePiece> whitePieces = new List<BasePiece>();
    public List<BasePiece> blackPieces = new List<BasePiece>();
    [SerializeField] private List<BasePiece> promotedPieces = new List<BasePiece>();
    [SerializeField] private List<BasePiece> kings = new List<BasePiece>();

    public bool isDraggingPiece = false;
    
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
        "Q",
        "K",
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

    public void Setup()
    {
        whitePieces = CreatePieces(Color.white);
        blackPieces = CreatePieces(Color.black);
        
        PlacePieces(1, 0, whitePieces);
        PlacePieces(6, 7, blackPieces);

        SwitchSides(Color.black);
    }

    private List<BasePiece> CreatePieces(Color teamColor)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        for (int i = 0; i < pieceOrder.Length; i++)
        {
            string key = pieceOrder[i];
            var piece = CreatePiece(teamColor, key);
            newPieces.Add(piece);

            if (piece.GetType() == typeof(King))
            {
                kings.Add(piece);
            }
        }

        return newPieces;
    }

    private BasePiece CreatePiece(Color teamColor, string key)
    {
        GameObject newPieceObj = Instantiate(piecePrefab);
        newPieceObj.transform.SetParent(transform);

        newPieceObj.transform.localScale = new Vector3(1, 1, 1);
        newPieceObj.transform.localRotation = Quaternion.identity;

        
        Type pieceType = pieceLibrary[key];

        BasePiece newPiece = (BasePiece) newPieceObj.AddComponent(pieceType);
        newPiece.Setup(teamColor, sprites.FirstOrDefault(x => x.pieceIdentifier == key));

        return newPiece;
    }

    public King GetKing(Color teamColor)
    {
        return (King)kings.FirstOrDefault(x => x.color == teamColor);
    }
    
    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePiece> pieces)
    {
        for (int i = 0; i < 8; i++)
        {
            pieces[i].Place(Board.instance.allCells[i, pawnRow]);
            pieces[i + 8].Place(Board.instance.allCells[i, royaltyRow]);
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
        King blackKing = GetKing(Color.black);
        King whiteKing = GetKing(Color.white);

        if (blackKing.IsCheckMate())
        {
            Debug.Log("White wins!");
        }
        else if (whiteKing.IsCheckMate())
        {
            Debug.Log("Black wins!");
        }

        bool isBlackTurn = color == Color.white;
        
        SetInteractive(whitePieces, !isBlackTurn);
        SetInteractive(blackPieces, isBlackTurn);

        foreach (BasePiece piece in promotedPieces)
        {
            bool isBlackPiece = piece.color == Color.black;
            bool isPartOfTeam = (isBlackPiece && isBlackTurn) || (!isBlackPiece && !isBlackTurn);

            piece.enabled = isPartOfTeam;
        }

        if (isBlackTurn && GameManager.instance.botGame)
        {
            BotMove();
        }
    }

    void BotMove()
    {
        // For now, get a random piece that can move and move it to a random position
        var pieces = blackPieces.Where(x => x.IsAlive()).ToList();

        pieces.ForEach(x => x.CheckPathing());
        var piece = pieces.Where(x => x.highlightedCells.Any()).ToList().Random();

        if (piece != null)
        {
            piece.targetCell = piece.highlightedCells.Random();
            piece.Move();
        }

        SwitchSides(Color.black);
    }
    
    public void ResetPieces()
    {
        foreach (BasePiece piece in promotedPieces)
        {
            piece.Kill();
            Destroy(piece.gameObject);
        }
        
        foreach (BasePiece piece in whitePieces)
        {
            piece.Reset();
        }
        
        foreach (BasePiece piece in blackPieces)
        {
            piece.Reset();
        }
    }

    public void EvolvePieces(Color teamColor)
    {
        if (teamColor == Color.black)
        {
            blackPieces.ForEach(x => x.Evolve());
        }
        else
        {
            whitePieces.ForEach(x => x.Evolve());
        }
    }

    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor)
    {
        int levelsToAdd = pawn.level - pawn.baseLevel;

        pawn.Kill(true);

        BasePiece promotedPiece = CreatePiece(teamColor, "Q");
        
        promotedPiece.Place(cell);

        if (teamColor == Color.black)
        {
            blackPieces.Add(promotedPiece);
        }
        else
        {
            whitePieces.Add(promotedPiece);
        }
        
        promotedPieces.Add(promotedPiece);
    }

    public void UpdateIsChecked(Color teamColor)
    {
        if (teamColor == Color.black)
        {
            whitePieces.ForEach(x => x.CheckPathing());
        }
        else
        {
            blackPieces.ForEach(x => x.CheckPathing());
        }
    }

    private List<Cell> IterateCells(int xPos, int yPos, int xIncrement, int yIncrement, int count)
    {
        List<Cell> cells = new List<Cell>();

        int curX = xPos;
        int curY = yPos;
        
        // Also add the possibility of taking the checking piece
        var currentCell = Board.instance.allCells[curX, curY];
        cells.Add(currentCell);
        
        for (int i = 0; i < Math.Abs(count); i++)
        {
            if (curX + xIncrement >= 0 && curX + xIncrement <= 7)
                curX += xIncrement;
            if (curY + yIncrement >= 0 && curY + yIncrement <= 7)
                curY += yIncrement;

            var cell = Board.instance.allCells[curX, curY];
            cells.Add(cell);
        }
        
        return cells;
    }
    
    public List<Cell> CheckingCellPath(Cell checkingPieceCell, King king)
    {
        Cell kingCell = king.CurrentCell;

        var xPos = checkingPieceCell.boardPosition.x;
        var yPos = checkingPieceCell.boardPosition.y;
        
        int xDiff = kingCell.boardPosition.x - xPos;
        int yDiff = kingCell.boardPosition.y - yPos;

        return IterateCells(xPos, yPos,
            (xDiff > 0) ? 1 : ((xDiff < 0) ? -1 : 0),
            (yDiff > 0) ? 1 : ((yDiff < 0) ? -1 : 0),
            (xDiff != 0) ? xDiff : yDiff);
    }

    public IEnumerable<Cell> GetCheckingCells(Color color)
    {
        return (color == Color.black)
            ? whitePieces.Where(x => x.isChecking && x.IsAlive()).SelectMany(x => CheckingCellPath(x.CurrentCell, GetKing(Color.black)))
            : blackPieces.Where(x => x.isChecking && x.IsAlive()).SelectMany(x => CheckingCellPath(x.CurrentCell, GetKing(Color.white)));
    }
    
    public bool CanCheckmateBePrevented(Color teamColor)
    {
        var checkingCells = GetCheckingCells(teamColor);

        var movements = (teamColor == Color.black)
            ? blackPieces.Where(x => x.IsAlive()).SelectMany(x => x.highlightedCells)
            : whitePieces.Where(x => x.IsAlive()).SelectMany(x => x.highlightedCells);

        var countInterceptions = movements.Count(x => checkingCells.Contains(x));

        return countInterceptions > 0;
    }
}
