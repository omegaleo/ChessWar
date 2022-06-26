using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Puzzle:ScriptableObject
{
    public Sprite image;
    public string puzzleName;
    public List<BasePiece> whitePieces;
    public List<BasePiece> blackPieces;
    public Color playerStartColor;
}