using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
    public bool isFirstMove = true;

    public enum AnimationType
    {
        CLAIM,
        ATTACK,
        SACRIFICE,
        ATTACKED,
        TEAR
    }

    private Animator animator;
    private Animator overlayAnimator;
    private Image animatorImage;

    public bool movementEnabled;

    public virtual void Setup(Color newColor, PieceSprite sprites)
    {
        color = newColor;

        blackSprite = sprites.blackSprite;
        whiteSprite = sprites.whiteSprite;
        eWhiteSprite = sprites.eWhiteSprite;
        eBlackSprite = sprites.eBlackSprite;
        
        GetComponent<Image>().sprite = (color == Color.black) ? blackSprite : whiteSprite;
        RectTransform = GetComponent<RectTransform>();

        overlayAnimator = transform.GetChild(0).GetComponent<Animator>();
        animatorImage = transform.GetChild(0).GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Handle animations for the piece in question
    /// </summary>
    /// <param name="type">Type of animation to execute</param>
    /// <param name="methodToCallWhenDone">Method to call when the animation is done, can be used to chain animations, example:
    /// When the sacrifice animation is done, call the claim animation in the other piece</param>
    /// <returns></returns>
    public IEnumerator ExecuteAnimation(AnimationType type, UnityAction methodToCallWhenDone = null)
    {
        // Call animator
        string animationName = "";
        var animatorToUse = overlayAnimator;
        
        switch (type)
        {
            case AnimationType.CLAIM:
                animationName = "ClaimSoul";
                transform.GetChild(0).GetComponent<Image>().sprite =
                    targetCell.currentPiece.GetComponent<Image>().sprite;
                break;
            case AnimationType.ATTACK:
                animationName = "Attack";
                break;
            case AnimationType.ATTACKED:
                animationName = "Attacked";
                break;
            case AnimationType.SACRIFICE:
                animatorToUse = animator;
                animationName = "Sacrifice";
                break;
            case AnimationType.TEAR:
                animationName = "Tear";
                break;
        }
        
        animatorToUse.SetBool(animationName, true);

        while (!animatorToUse.AnimatorIsPlaying())
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        // Wait for animation to stop
        yield return animatorToUse.WaitForAnimation();

        animatorToUse.SetBool(animationName, false);
        
        if (methodToCallWhenDone != null)
        {
            methodToCallWhenDone.Invoke();
        }
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

            if (state != CellState.OutOfBounds)
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
                        else
                        {
                            isChecking = false;
                            CurrentCell.outlineImage.enabled = false;
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
        return piece.GetType() == typeof(King) && piece.color != color && this.GetType() != typeof(King);
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
        highlightedCells.Clear();
        
        Board.instance.ResetCellOutlines();
        
        if (PieceManager.instance.EvolvedQueenSecondMove(color) && (GetType() != typeof(Queen) || !evolved))
        {
            return;
        }

        isChecking = false;
        CurrentCell.outlineImage.enabled = false;

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

        CheckedFilter();
    }

    internal void CheckedFilter()
    {
        if (PieceManager.instance.GetKing(color).IsChecked() && this.GetType() != typeof(King))
        {
            var checkingCells = PieceManager.instance.GetCheckingCells(color).ToList();
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

    internal virtual void Move()
    {
        bool isSecondaryPlayer = color == PieceManager.instance.player2Color;
        if (targetCell.currentPiece != null)
        {
            if (targetCell.currentPiece.color == color)
            {
                if (!(GameManager.instance.botGame && isSecondaryPlayer))
                {
                    SFXManager.instance.Play("soulSFX");
                }

                targetCell.currentPiece.ExecuteAnimation(AnimationType.TEAR);
                StartCoroutine(targetCell.currentPiece.ExecuteAnimation(AnimationType.SACRIFICE, () =>
                {
                    targetCell.currentPiece.GetComponent<Image>().enabled = false;
                    ExecuteMovement(targetCell);
                    
                    /*StartCoroutine(ExecuteAnimation(AnimationType.CLAIM, () =>
                    {
                        ExecuteMovement(targetCell);
                    }));*///Disabled for now
                }));
            }
            else
            {
                SFXManager.instance.Play("slashSFX");
                StartCoroutine(targetCell.currentPiece.ExecuteAnimation(AnimationType.ATTACKED, () =>
                {
                    ExecuteMovement(targetCell);
                }));
            }
        }
        else
        {
            ExecuteMovement(targetCell);
        }
    }

    protected virtual void ExecuteMovement(Cell cell)
    {
        if (cell == null)
            return;

        SFXManager.instance.Play("pieceMoveSFX");

        CellState state = CellState.None;
        state = Board.instance.ValidateCell(targetCell.boardPosition.x, targetCell.boardPosition.y, this);

        if (state == CellState.Friendly)
        {
            // Sacrifice a piece
            var piece = targetCell.currentPiece;
        
            this.level += piece.level;
            CheckEvolved();
        }
        
        
        cell.RemovePiece();

        CurrentCell.currentPiece = null;
        CurrentCell.outlineImage.enabled = false;
        CurrentCell = cell;
        CurrentCell.currentPiece = this;

        transform.position = CurrentCell.transform.position;

        // Check if any of the kings is in check
        PieceManager.instance.UpdatePaths();

        targetCell = null;
        
        move++;
        
        highlightedCells.ForEach((x) =>
        {
            x.outlineImage.enabled = false;
        });

        PieceManager.instance.CheckGameOver(color);

        StartCoroutine(WaitToChangeSides());
    }

    IEnumerator WaitToChangeSides()
    {
        yield return new WaitForSeconds(1f);
        if (!moveTwice || move > 1)
        {
            move = 0;
            PieceManager.instance.SwitchSides(color);
        }
        else if (GameManager.instance.botGame && moveTwice && color == PieceManager.instance.player2Color)
        {
            BotAI.Move(color);
        }
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
            
            InformationPanelManager.instance.OpenCurrent(this);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (!PieceManager.instance.isDraggingPiece)
        {
            ClearCells();
            InformationPanelManager.instance.CloseCurrent();
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        
        if (!movementEnabled)
        {
            return;
        }
        
        CheckPathing();
        
        ShowCells();
        PieceManager.instance.isDraggingPiece = true;
        
        InformationPanelManager.instance.OpenCurrent(this);
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
        if (color != PieceManager.instance.currentColor)
        {
            return;
        }
        
        base.OnDrag(eventData);
        
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z += Camera.main.nearClipPlane;

        transform.position = mousePos;
        
        targetCell = null;
        
        foreach (Cell cell in highlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.rectTransform, mousePos))
            {
                targetCell = cell;
                //transform.position = targetCell.gameObject.transform.position; // Code used to snap the piece's position to the cell
                if (targetCell.currentPiece != null)
                {
                    InformationPanelManager.instance.OpenTarget(targetCell.currentPiece, targetCell.currentPiece.color == color);
                }
                
                break;
            }
        }
    }

    public virtual string GetDescription()
    {
        string description = "";
        
        if (evolved)
        {
            
        }
        else
        {
            description = $"{Environment.NewLine}Evolves at level {evolveLevel}{Environment.NewLine}";
        }

        return description;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        PieceManager.instance.isDraggingPiece = false;
        
        InformationPanelManager.instance.CloseCurrent();
        InformationPanelManager.instance.CloseTarget();
        
        transform.position = CurrentCell.gameObject.transform.position;
        
        ClearCells();

        if (!targetCell)
        {
            return;
        }

        Move();
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
