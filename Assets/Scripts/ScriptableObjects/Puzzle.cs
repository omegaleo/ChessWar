using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "puzzle", menuName = "ChessWar/Puzzle")]
public class Puzzle:ScriptableObject
{
    public Sprite image;
    public string puzzleName;
    public List<PuzzlePiecePosition> whitePieces;
    public List<PuzzlePiecePosition> blackPieces;
    public Color playerStartColor;
}

[Serializable]
public class PuzzlePiecePosition
{
    public int x;
    public int y;
    public string pieceType;
    public int startLevel;
    public bool firstMove = false; // Normally in puzzles pieces aren't on their first move
}