using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePiece : EventTrigger
{
    public Color color = Color.clear;

    protected Cell OriginalCell = null;
    public Cell CurrentCell = null;

    protected RectTransform RectTransform;

    public Sprite whiteSprite;
    public Sprite blackSprite;
    public Sprite eWhiteSprite;
    public Sprite eBlackSprite;

    protected Vector3Int movement = Vector3Int.one;
    public List<Cell> highlightedCells = new List<Cell>();

    public Cell targetCell;

    public int level;
    public int baseLevel;
    public int evolveLevel = 3;
    public bool evolved = false;
    public bool bypassMovement = false; // For rook evolved form
    public bool isChecking = false;
    public bool moveTwice = false;
    public int move = 0;

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

    public virtual void Place(Cell newCell)
    {
        // Cell stuff
        CurrentCell = newCell;
        OriginalCell = newCell;
        CurrentCell.currentPiece = this;
        SFXManager.instance.Play("pieceMoveSFX");
        // Object stuff
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    #region Movement

    public IEnumerable<Cell> GetCellPath(int xDir, int yDir, int movement)
    {
        List<Cell> cells = new List<Cell>();
        
        int currentX = CurrentCell.boardPosition.x;
        int currentY = CurrentCell.boardPosition.y;

        for (int i = 1; i <= movement; i++)
        {
            currentX += xDir;
            currentY += yDir;
            
            CellState state = CellState.None;
            state = Board.instance.ValidateCell(currentX, currentY, this);

            if (state == CellState.Enemy || state == CellState.Free || state == CellState.Friendly)
            {
                var possibleTarget = Board.instance.allCells[currentX, currentY];

                if (possibleTarget.currentPiece != null)
                {
                    if (IsValidMovement(possibleTarget.currentPiece))
                    {
                        cells.Add(possibleTarget);

                        if (state != CellState.Friendly || !bypassMovement) // Check if upgraded rook and piece blocking is a friendly piece
                        {
                            break;
                        }
                    }
                    else
                    {
                        var piece = possibleTarget.currentPiece;
                        if (ValidChecking(piece))
                        {
                            isChecking = true;
                            CurrentCell.outlineImage.enabled = true;
                        }

                        break;
                    }
                }

                cells.Add(possibleTarget);
            }
            else
            {
                break;
            }
        }

        return cells;
    }

    public bool ValidChecking(BasePiece piece)
    {
        return piece.GetType() == typeof(King) && piece.color != color && this.GetType() != typeof(King) && this.GetType() != typeof(Pawn);
    }

    public void CreateCellPath(int xDir, int yDir, int movement)
    {
        highlightedCells.AddRange(GetCellPath(xDir, yDir, movement).ToList());
    }

    public virtual void CheckEvolved()
    {
        if (level >= evolveLevel && !evolved)
        {
            Evolve();
        }
    }

    public virtual void Evolve()
    {
        GetComponent<Image>().sprite = (color == Color.black) ? eBlackSprite : eWhiteSprite;
        evolved = true;
        
        PieceManager.instance.CheckIfKingCanEvolve(color);
    }

    public virtual void CheckPathing()
    {
        isChecking = false;
        CurrentCell.outlineImage.enabled = false;
        highlightedCells.Clear();
        
        if (!IsAlive())
        {
            return;
        }
        
        // Horizontal
        CreateCellPath(1, 0, movement.x);
        CreateCellPath(-1, 0, movement.x);
        
        // Vertical
        CreateCellPath(0, 1, movement.y);
        CreateCellPath(0, -1, movement.y);
        
        // Upper Diagonal
        CreateCellPath(1, 1, movement.z);
        CreateCellPath(-1, 1, movement.z);
        
        // Lower Diagonal
        CreateCellPath(1, -1, movement.z);
        CreateCellPath(-1, -1, movement.z);

        if (PieceManager.instance.GetKing(color).IsChecked() && this.GetType() != typeof(King))
        {
            var checkingCells = PieceManager.instance.GetCheckingCells(color);
            highlightedCells = highlightedCells.Where(x => checkingCells.Contains(x)).ToList();
        }
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

    public virtual void Move()
    {
        bool isSecondaryPlayer = color == PieceManager.instance.player2Color;
        if (targetCell.currentPiece != null && targetCell.currentPiece.color == color && !(GameManager.instance.botGame && isSecondaryPlayer))
        {
            SFXManager.instance.Play("soulSFX");
        }
        else
        {
            SFXManager.instance.Play("pieceMoveSFX");
        }
        
        targetCell.RemovePiece();

        CurrentCell.currentPiece = null;

        CurrentCell = targetCell;
        CurrentCell.currentPiece = this;

        transform.position = CurrentCell.transform.position;
        
        // Check if any of the kings is in check
        PieceManager.instance.UpdateIsChecked(Color.white);
        PieceManager.instance.UpdateIsChecked(Color.black);

        targetCell = null;
    }

    #endregion

    #region Events

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (!PieceManager.instance.isDraggingPiece)
        {
            CheckPathing();
        
            ShowCells();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        
        if (!PieceManager.instance.isDraggingPiece)
            ClearCells();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        
        CheckPathing();
        
        ShowCells();
        PieceManager.instance.isDraggingPiece = true;
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
        PieceManager.instance.isDraggingPiece = false;
        ClearCells();

        if (!targetCell)
        {
            transform.position = CurrentCell.gameObject.transform.position;
            return;
        }
        
        CellState state = CellState.None;
        state = Board.instance.ValidateCell(targetCell.boardPosition.x, targetCell.boardPosition.y, this);

        if (state == CellState.Friendly)
        {
            // Sacrifice a piece
            var piece = targetCell.currentPiece;
        
            if (IsValidMovement(piece))
            {
                this.level += piece.level;
                CheckEvolved();
            }
            else
            {
                transform.position = CurrentCell.gameObject.transform.position;
                return;
            }
        }
        
        Move();
        move++;
        
        if (!moveTwice || move > 1)
        {
            move = 0;
            PieceManager.instance.SwitchSides(color);
        }
    }

    public virtual bool IsValidMovement(BasePiece piece)
    {
        return piece.level <= this.level && this.GetType() != typeof(King) && piece.GetType() != typeof(King);
    }

    #endregion

    public virtual void Reset()
    {
        Kill();
        Place(OriginalCell);
        isChecking = false;
        CurrentCell.outlineImage.enabled = false;
    }

    public virtual void Kill(bool promotion = false)
    {
        CurrentCell.currentPiece = null;
        CurrentCell.bloodSplatterImage.enabled = !promotion;
        gameObject.SetActive(false);
    }

    public bool IsAlive()
    {
        return gameObject.activeSelf;
    }
}
