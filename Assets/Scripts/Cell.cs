using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image outlineImage;
    public Image checkedImage;
    public Vector2Int boardPosition = Vector2Int.zero;
    public Board board;
    public RectTransform rectTransform;

    public BasePiece currentPiece;

    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        boardPosition = newBoardPosition;
        board = newBoard;

        rectTransform = GetComponent<RectTransform>();
    }

    public void RemovePiece()
    {
        if (currentPiece != null)
        {
            currentPiece.Kill();
        }
    }
}
