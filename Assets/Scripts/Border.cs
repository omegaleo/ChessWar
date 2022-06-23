using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Border : InstancedBehaviour<Border>
{
    [SerializeField] private GameObject cellPrefab;
    public BorderCell[,] allCells = new BorderCell[20, 20];

    [SerializeField] private float size = 87.5f;
    [SerializeField] private float off = 43.75f;

    public static Border instance;

    private List<char> characters = new List<char>()
    {
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
    };

    [SerializeField] private Sprite horizontalSprite;
    [SerializeField] private Sprite verticalSprite;
    [SerializeField] private Sprite bottomLeftSprite;
    [SerializeField] private Sprite bottomRightSprite;
    [SerializeField] private Sprite topLeftSprite;
    [SerializeField] private Sprite topRightSprite;

    private void Start()
    {
        // Create all cells
        for (int y = 0; y < 10; y++)
        {
            // Create cell
            int x = 0;
            var newCell = Instantiate(cellPrefab, transform);
            newCell.name = $"Cell {x}, {y}";
                
            // Position
            var rectTransform = newCell.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(x, (y * size));

            // Setup
            allCells[x, y] = newCell.GetComponent<BorderCell>();
            if (y > 0 && y < 9)
            {
                allCells[x, y].Set(y.ToString());
            }
            
            // Create cell
            x = 9;
            var newCellRight = Instantiate(cellPrefab, transform);
            newCellRight.name = $"Cell {x}, {y}";
                
            // Position
            var rightRectTransform = newCellRight.GetComponent<RectTransform>();
            rightRectTransform.anchoredPosition = new Vector2((x * size), (y * size));
            
            newCellRight.GetComponent<Image>().sprite = bottomLeftSprite;
            
            // Setup
            allCells[x, y] = newCellRight.GetComponent<BorderCell>();
            if (y > 0 && y < 9)
            {
                allCells[x, y].Set(y.ToString());
            }
            
            if (y == 0)
            {
                newCell.GetComponent<Image>().sprite = bottomLeftSprite;
                newCellRight.GetComponent<Image>().sprite = bottomRightSprite;
            }
            else if (y == 9)
            {
                newCell.GetComponent<Image>().sprite = topLeftSprite;
                newCellRight.GetComponent<Image>().sprite = topRightSprite;
            }
            else
            {
                newCell.GetComponent<Image>().sprite = verticalSprite;
                newCellRight.GetComponent<Image>().sprite = verticalSprite;
            }
        }
        
        for (int x = 1; x < 9; x++)
        {
            // Create cell
            int y = 0;
            var newCell = Instantiate(cellPrefab, transform);
            newCell.name = $"Cell {x}, {y}";
                
            // Position
            var rectTransform = newCell.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2((x * size), y);
            
            newCell.GetComponent<Image>().sprite = horizontalSprite;
            // Setup
            allCells[x, y] = newCell.GetComponent<BorderCell>();
            if (x > 0 && x < 9)
            {
                allCells[x, y].Set(characters[x - 1].ToString());
            }
            
            // Create cell
            y = 9;
            var newCellRight = Instantiate(cellPrefab, transform);
            newCellRight.name = $"Cell {x}, {y}";
                
            // Position
            var rightRectTransform = newCellRight.GetComponent<RectTransform>();
            rightRectTransform.anchoredPosition = new Vector2((x * size), (y * size));
            newCellRight.GetComponent<Image>().sprite = horizontalSprite;
            // Setup
            allCells[x, y] = newCellRight.GetComponent<BorderCell>();
            if (x > 0 && x < 9)
            {
                allCells[x, y].Set(characters[x - 1].ToString());
            }
        }
    }
}
