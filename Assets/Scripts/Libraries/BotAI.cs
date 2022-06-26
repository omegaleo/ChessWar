using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BotAI
{
    private static List<PieceValue> values = new List<PieceValue>()
    {
        new PieceValue() {type = typeof(Pawn), value = 10},
        new PieceValue() {type = typeof(Knight), value = 30},
        new PieceValue() {type = typeof(Bishop), value = 30},
        new PieceValue() {type = typeof(Rook), value = 50},
        new PieceValue() {type = typeof(Queen), value = 90},
        new PieceValue() {type = typeof(King), value = 100}
    };

    public static void Move(Color color)
    {
        bool sacrifice = new System.Random().Next(0, 100) <= 10;
        
        var allPossibleMoves = PieceManager.instance.GetAllPossibleMoves(color).ToList();
        var pieces = PieceManager.instance.GetPieces(color);

        var bestMove = allPossibleMoves.Random();
        var pieceToMove = pieces.FirstOrDefault(x => x.highlightedCells.Any(x => x == bestMove));
        var bestMoveValue = GetValue(bestMove, GetMultiplier(color, bestMove, pieceToMove));

        foreach (Cell cell in allPossibleMoves)
        {
            if (IsSameColor(color, cell) && !sacrifice)
            {
                continue;
            }

            var piece = pieces.FirstOrDefault(x => x.highlightedCells.Any(x => x == cell));

            var value = GetValue(cell, GetMultiplier(color, cell, piece));

            if (value > bestMoveValue)
            {
                bestMove = cell;
                bestMoveValue = value;
                pieceToMove = piece;
            }
        }

        if (pieceToMove != null)
        {
            pieceToMove.targetCell = bestMove;
            pieceToMove.Move();
        }
    }

    private static int GetMultiplier(Color color, Cell cell, BasePiece piece)
    {
        return (IsSameColor(color, cell) 
            ? (piece.evolved) 
                ? -1 : 1
            : 1);
    }

    private static bool IsSameColor(Color color, Cell cell)
    {
        return cell.currentPiece != null && cell.currentPiece.color == color;
    }

    private static int GetValue(Cell cell, int multiplier)
    {
        int? value = 0;

        if (cell.currentPiece != null)
        {
            value = values.FirstOrDefault(x => x.type == cell.currentPiece.GetType())?.value;
        }
        
        return value * multiplier ?? 0;
    }
    
    /*void BotMove()
    {
        // For now, get a random piece that can move and move it to a random position
        var teamPieces = (player2Color == Color.black) ? blackPieces : whitePieces;
        
        var pieces = teamPieces.Where(x => x.IsAlive()).ToList();

        pieces.ForEach(x => x.CheckPathing());
        var piece = pieces.Where(x => x.highlightedCells.Any()).ToList().Random();

        if (piece != null)
        {
            piece.targetCell = piece.highlightedCells.Random();
            piece.Move();
        }

        SwitchSides(player2Color);
    }*/
}

public class PieceValue
{
    public Type type;
    public int value;

    public int evolvedValue
    {
        get
        {
            return Mathf.CeilToInt(value * 1.5f); // Return 150% of the piece's value if evolved
        }
    }
}