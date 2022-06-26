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
        var r = new System.Random();
        if (GameManager.instance.difficulty == GameManager.Difficulty.Easy)
        {
            bool randomMove = r.Next(0, 100) <= 40;

            if (randomMove)
            {
                RandomMove(color);
            }
        }

        int percentageToSacrifice = 0;

        switch (GameManager.instance.difficulty)
        {
            case GameManager.Difficulty.Easy:
                percentageToSacrifice = 2;
                break;
            case GameManager.Difficulty.Normal:
                percentageToSacrifice = 5;
                break;
            case GameManager.Difficulty.Hard:
                percentageToSacrifice = 15;
                break;
        }
        
        bool sacrifice =  r.Next(0, 100) <= percentageToSacrifice;
        
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
            : (piece.evolved) ? 3 : 2);
    }

    private static bool IsSameColor(Color color, Cell cell)
    {
        return cell.currentPiece != null && cell.currentPiece.color == color;
    }

    private static int GetValue(Cell cell, int multiplier)
    {
        int value = 0;

        if (cell.currentPiece != null)
        {
            var pieceValue = values.FirstOrDefault(x => x.type == cell.currentPiece.GetType());

            if (pieceValue != null)
            {
                value = (cell.currentPiece.evolved) ? pieceValue.evolvedValue : pieceValue.value;
            }
        }
        
        return value * multiplier;
    }
    
    private static void RandomMove(Color color)
    {
        var allPossibleMoves = PieceManager.instance.GetAllPossibleMoves(color).ToList();
        var pieces = PieceManager.instance.GetPieces(color);
        var bestMove = allPossibleMoves.Random();
        var piece = pieces.FirstOrDefault(x => x.highlightedCells.Any(x => x == bestMove));

        if (piece != null)
        {
            piece.targetCell = piece.highlightedCells.Random();
            piece.Move();
        }
    }
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