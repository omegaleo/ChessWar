using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    public Cell[,] allCells = new Cell[8, 8];

    public static Board instance;

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

    private void Start()
    {
        // Create all cells
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                // Create cell
                var newCell = Instantiate(cellPrefab, transform);
                newCell.name = $"Cell {x}, {y}";
                
                // Position
                var rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                
                // Setup
                allCells[x, y] = newCell.GetComponent<Cell>();
                allCells[x, y].Setup(new Vector2Int(x, y),this);
            }
        }
        
        // Color cells
        for (int x = 0; x < 8; x+=2)
        {
            for (int y = 0; y < 8; y++)
            {
                // Offset for every other line
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;
                
                // Color
                allCells[finalX, y].GetComponent<Image>().color = Color.black;
            }
        }
    }

    public CellState ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {
        if (targetX < 0 || targetX > 7 || targetY < 0 || targetY > 7)
        {
            return CellState.OutOfBounds;
        }

        Cell targetCell = allCells[targetX, targetY];

        if (targetCell.currentPiece != null)
        {
            if (checkingPiece.color == targetCell.currentPiece.color)
            {
                return CellState.Friendly;
            }
            else
            {
                return CellState.Enemy;
            }
        }

        return CellState.Free;
    }
}
