using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PieceManager : InstancedBehaviour<PieceManager>
{
    public bool isKingAlive = true;
    
    [SerializeField] private GameObject piecePrefab;
    public List<BasePiece> whitePieces = new List<BasePiece>();
    public List<BasePiece> blackPieces = new List<BasePiece>();
    [SerializeField] private List<BasePiece> promotedPieces = new List<BasePiece>();
    [SerializeField] private List<BasePiece> kings = new List<BasePiece>();

    public bool isDraggingPiece = false;

    public Color player1Color = Color.white;
    public Color player2Color = Color.black;
    public Color currentColor;
    
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

    public List<PieceSprite> sprites = new List<PieceSprite>()
    {
        new PieceSprite() {pieceIdentifier = "P"},
        new PieceSprite() {pieceIdentifier = "R"},
        new PieceSprite() {pieceIdentifier = "KN"},
        new PieceSprite() {pieceIdentifier = "B"},
        new PieceSprite() {pieceIdentifier = "K"},
        new PieceSprite() {pieceIdentifier = "Q"},
    };

    public void Setup()
    {
        player1Color = new List<Color>() {Color.white, Color.black}.Random();
        player2Color = (player1Color == Color.white) ? Color.black : Color.white;
        
        whitePieces = CreatePieces(Color.white);
        blackPieces = CreatePieces(Color.black);
        
        PlacePieces(1, 0, (player1Color == Color.white)? whitePieces : blackPieces);
        PlacePieces(6, 7, (player1Color != Color.white)? whitePieces : blackPieces);

        SwitchSides(Color.black);
    }

    public void SetupPuzzle(Puzzle puzzle)
    {
        player1Color = puzzle.playerStartColor;
        player2Color = (player1Color == Color.white) ? Color.black : Color.white;

        foreach (var piece in puzzle.blackPieces)
        {
            var blackPiece = CreatePiece(Color.black, piece.pieceType);
            blackPiece.Place(Board.instance.allCells[piece.x, piece.y]);

            blackPiece.level = piece.startLevel;
            blackPiece.CheckEvolved();

            blackPiece.isFirstMove = piece.firstMove;
            
            if (blackPiece.GetType() == typeof(King))
            {
                kings.Add(blackPiece);
            }
            
            blackPieces.Add(blackPiece);
        }
        
        foreach (var piece in puzzle.whitePieces)
        {
            var whitePiece = CreatePiece(Color.white, piece.pieceType);
            whitePiece.Place(Board.instance.allCells[piece.x, piece.y]);

            whitePiece.level = piece.startLevel;
            whitePiece.CheckEvolved();
            
            whitePiece.isFirstMove = piece.firstMove;
            
            if (whitePiece.GetType() == typeof(King))
            {
                kings.Add(whitePiece);
            }
            
            whitePieces.Add(whitePiece);
        }
        
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
        return (King)kings.FirstOrDefault(x =>  x.color == teamColor);
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
            piece.movementEnabled = value;
        }
    }

    public void SwitchSides(Color color)
    {
        King blackKing = GetKing(Color.black);
        King whiteKing = GetKing(Color.white);

        Board.instance.ClearSelectedCells();
        
        // Check if Kings are checked and evolved to check if they can switch with a Rook
        if (blackKing.IsChecked() && blackKing.evolved)
        {
            blackKing.TrySwitchRook();
        }

        if (whiteKing.IsChecked() && whiteKing.evolved)
        {
            whiteKing.TrySwitchRook();
        }
        
        bool gameOver = CheckGameOver(color);
        
        if (!gameOver)
        {
            bool isBlackTurn = color == Color.white;

            currentColor = (isBlackTurn) ? Color.black : Color.white;
            
            SetInteractive(whitePieces, !isBlackTurn);
            SetInteractive(blackPieces, isBlackTurn);

            foreach (BasePiece piece in promotedPieces)
            {
                bool isBlackPiece = piece.color == Color.black;
                bool isPartOfTeam = (isBlackPiece && isBlackTurn) || (!isBlackPiece && !isBlackTurn);

                piece.enabled = isPartOfTeam;
            }

            if (((isBlackTurn && player2Color == Color.black) || (!isBlackTurn && player2Color == Color.white)) &&
                GameManager.instance.botGame)
            {
                BotAI.Move(player2Color);
            }
        }
    }

    public bool CheckGameOver(Color color)
    {
        bool gameOver = false;
        King blackKing = GetKing(Color.black);
        King whiteKing = GetKing(Color.white);
        
        // Check if CheckMate
        if (blackKing.IsCheckMate())
        {
            gameOver = true;
            if (player1Color == Color.black)
            {
                CheckmateScreen.instance.Open((GameManager.instance.botGame) ? "DEFEAT" : "WHITE WINS");
            }
            else
            {
                CheckmateScreen.instance.Open((GameManager.instance.botGame) ? "VICTORY" : "WHITE WINS");
            }
        }
        else if (whiteKing.IsCheckMate())
        {
            gameOver = true;
            if (player1Color == Color.white)
            {
                CheckmateScreen.instance.Open((GameManager.instance.botGame) ? "DEFEAT" : "BLACK WINS");
            }
            else
            {
                CheckmateScreen.instance.Open((GameManager.instance.botGame) ? "VICTORY" : "BLACK WINS");
            }
        }

        return gameOver;
    }

    public void ResetGame()
    {
        foreach (BasePiece piece in promotedPieces)
        {
            piece.Kill();
            Destroy(piece.gameObject);
        }
        
        foreach (BasePiece piece in whitePieces)
        {
            piece.Kill();
            Destroy(piece.gameObject);
        }
        
        foreach (BasePiece piece in blackPieces)
        {
            piece.Kill();
            Destroy(piece.gameObject);
        }
        
        promotedPieces.Clear();
        whitePieces.Clear();
        blackPieces.Clear();
        kings.Clear();

        foreach (Cell cell in Board.instance.allCells)
        {
            cell.checkedImage.enabled = false;
            cell.bloodSplatterImage.enabled = false;
        }
        
        Setup();
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

    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor, string piece)
    {
        int additionalLevels = pawn.GetAdditionalLevels();
        pawn.Kill(true);
        
        BasePiece promotedPiece = CreatePiece(teamColor, piece);
        
        promotedPiece.Place(cell);
        promotedPiece.level += additionalLevels;
        promotedPiece.CheckEvolved();

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

    public void UpdatePaths()
    {
        whitePieces.ForEach(x => x.CheckPathing());
        blackPieces.ForEach(x => x.CheckPathing());
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

        var movements = GetAllFriendlyPossibleMovements(teamColor);

        var countInterceptions = movements.Count(x => checkingCells.Contains(x));

        return countInterceptions > 0;
    }

    public IEnumerable<Cell> GetAllPossibleMoves(Color color)
    {
        UpdatePaths();
        
        return GetPieces(color).SelectMany(x => x.highlightedCells);
    }

    public IEnumerable<BasePiece> GetPieces(Color color)
    {
        return (color == Color.black)
            ? blackPieces.Where(x => x.IsAlive())
            : whitePieces.Where(x => x.IsAlive());
    }

    /// <summary>
    /// Get a list of all possible movement that a friendly piece might do
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public IEnumerable<Cell> GetAllFriendlyPossibleMovements(Color color)
    {
        var movements = (color == Color.black)
            ? blackPieces.Where(x => x.IsAlive()).SelectMany(x => x.highlightedCells)
            : whitePieces.Where(x => x.IsAlive()).SelectMany(x => x.highlightedCells);

        return movements;
    }

    /// <summary>
    /// Get a list of all possible movement that an opposing piece might do
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public IEnumerable<Cell> GetAllOpposingPossibleMovements(Color color)
    {
        var movements = (color == Color.white)
            ? blackPieces.Where(x => x.IsAlive()).SelectMany(x => x.highlightedCells)
            : whitePieces.Where(x => x.IsAlive()).SelectMany(x => x.highlightedCells);

        return movements;
    }

    /// <summary>
    /// Method to check if the King belonging to that color can evolve
    /// </summary>
    /// <param name="teamColor"></param>
    public void CheckIfKingCanEvolve(Color teamColor)
    {
        King king = GetKing(teamColor);

        if (king == null) return;
        
        if (king.evolved) return; // Skip if the king is already evolved

        int count = (teamColor == Color.black) ? blackPieces.Count(x => x.evolved) : whitePieces.Count(x => x.evolved) ;

        if (count >= 3)
        {
            king.Evolve();
        }
    }

    /// <summary>
    /// Get a list of Rooks available
    /// </summary>
    /// <param name="teamColor"></param>
    /// <returns></returns>
    public List<BasePiece> GetRooks(Color teamColor)
    {
        if (teamColor == Color.black)
        {
            return blackPieces.Where(x => x.GetType() == typeof(Rook) && x.IsAlive()).ToList();
        }
        else
        {
            return whitePieces.Where(x => x.GetType() == typeof(Rook) && x.IsAlive()).ToList();
        }
    }

    /// <summary>
    /// Used to evaluate if there's an evolved queen and she's going for the 2nd move
    /// </summary>
    /// <param name="teamColor"></param>
    /// <returns></returns>
    public bool EvolvedQueenSecondMove(Color teamColor)
    {
        return (teamColor == Color.black)
            ? blackPieces.Any(x => x.GetType() == typeof(Queen) && x.evolved && x.move == 1)
            : whitePieces.Any(x => x.GetType() == typeof(Queen) && x.evolved && x.move == 1);
    }
}
