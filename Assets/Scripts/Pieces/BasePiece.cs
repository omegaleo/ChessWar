using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePiece : EventTrigger
{
    public Color color = Color.clear;

    protected Cell OriginalCell = null;
    protected Cell CurrentCell = null;

    protected RectTransform RectTransform;

    public Sprite whiteSprite;
    public Sprite blackSprite;
    public Sprite eWhiteSprite;
    public Sprite eBlackSprite;

    protected Vector3Int movement = Vector3Int.one;
    protected List<Cell> highlightedCells = new List<Cell>();

    protected Cell targetCell;

    public float level;
    
    public virtual void Setup(Color newColor, PieceSprite sprites)
    {
        color = newColor;

        blackSprite = sprites.blackSprite;
        whiteSprite = sprites.whiteSprite;
        eWhiteSprite = sprites.eWhiteSprite;
        eBlackSprite = sprites.eBlackSprite;
        
        GetComponent<Image>().sprite = (color == Color.black) ? blackSprite : whiteSprite;
        RectTransform = GetComponent<RectTransform>();
    }

    public void Place(Cell newCell)
    {
        // Cell stuff
        CurrentCell = newCell;
        OriginalCell = newCell;
        CurrentCell.currentPiece = this;
        
        // Object stuff
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    #region Movement

    private void CreateCellPath(int xDir, int yDir, int movement)
    {
        int currentX = CurrentCell.boardPosition.x;
        int currentY = CurrentCell.boardPosition.y;

        for (int i = 1; i <= movement; i++)
        {
            currentX += xDir;
            currentY += yDir;

            CellState state = CellState.None;
            state = CurrentCell.board.ValidateCell(currentX, currentY, this);

            if (state == CellState.Enemy || state == CellState.Free || state == CellState.Friendly)
            {
                highlightedCells.Add(CurrentCell.board.allCells[currentX, currentY]);
            }
            else
            {
                break;
            }
        }
    }

    protected virtual void CheckEvolved()
    {
        GetComponent<Image>().sprite = (color == Color.black) ? eBlackSprite : eWhiteSprite;
    }
    
    protected virtual void CheckPathing()
    {
        highlightedCells.Clear();
        
        // Horizontal
        CreateCellPath(1, 0, movement.x);
        CreateCellPath(-1, 0, movement.x);
        
        // Vertical
        CreateCellPath(0, 1, movement.y);
        CreateCellPath(0, -1, movement.y);
        
        // Upper Diagonal
        CreateCellPath(1, 1, movement.x);
        CreateCellPath(-1, 1, movement.x);
        
        // Lower Diagonal
        CreateCellPath(1, -1, movement.x);
        CreateCellPath(-1, -1, movement.x);
    }

    protected void ShowCells()
    {
        foreach (Cell cell in highlightedCells)
        {
            cell.outlineImage.enabled = true;
        }
    }

    protected void ClearCells()
    {
        foreach (Cell cell in highlightedCells)
        {
            cell.outlineImage.enabled = false;
        }
    }

    protected virtual void Move()
    {
        targetCell.RemovePiece();

        CurrentCell.currentPiece = null;

        CurrentCell = targetCell;
        CurrentCell.currentPiece = this;

        transform.position = CurrentCell.transform.position;
        targetCell = null;
    }

    #endregion

    #region Events

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        
        CheckPathing();
        
        ShowCells();
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        transform.position += ((Vector3) eventData.delta) * Time.deltaTime;

        targetCell = null;
        
        foreach (Cell cell in highlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.rectTransform, Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                targetCell = cell;
                break;
            }
        }
    }
    
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        
        ClearCells();

        if (!targetCell)
        {
            transform.position = CurrentCell.gameObject.transform.position;
            return;
        }
        
        CellState state = CellState.None;
        state = CurrentCell.board.ValidateCell(targetCell.boardPosition.x, targetCell.boardPosition.y, this);

        if (state == CellState.Friendly)
        {
            var piece = targetCell.currentPiece;
        
            if (piece.level <= this.level)
            {
                this.level += (piece.level / this.level);
                CheckEvolved();
            }
        }
        
        Move();
        
        PieceManager.instance.SwitchSides(color);
    }

    #endregion

    public void Reset()
    {
        Kill();
        Place(OriginalCell);
    }

    public virtual void Kill()
    {
        CurrentCell.currentPiece = null;
        gameObject.SetActive(false);
    }
}
