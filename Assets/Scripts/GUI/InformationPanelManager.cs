using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanelManager : InstancedBehaviour<InformationPanelManager>
{
    [Header("Current")] 
    [SerializeField] private GameObject currentPanel;
    [SerializeField] private Image currentIcon;
    [SerializeField] private TMP_Text currentDescription;
    
    [Header("Target")] 
    [SerializeField] private GameObject targetPanel;
    [SerializeField] private Image targetIcon;
    [SerializeField] private TMP_Text targetDescription;

    public void OpenCurrent(BasePiece piece)
    {
        currentPanel.SetActive(true);
        
        currentIcon.sprite = (piece.color == Color.black) ? 
            (piece.evolved) ? piece.eBlackSprite : piece.blackSprite :
            (piece.evolved) ? piece.eWhiteSprite : piece.whiteSprite;

        currentDescription.text = GetDescription(piece);
    }

    public void CloseCurrent()
    {
        currentPanel.SetActive(false);
    }

    public void OpenTarget(BasePiece piece, bool isSacrifice = false)
    {
        targetPanel.SetActive(true);
        
        targetIcon.sprite = (piece.color == Color.black) ? 
            (piece.evolved) ? piece.eBlackSprite : piece.blackSprite :
            (piece.evolved) ? piece.eWhiteSprite : piece.whiteSprite;

        targetDescription.text = GetDescription(piece, isSacrifice);
    }

    public void CloseTarget()
    {
        targetPanel.SetActive(false);
    }

    private string GetDescription(BasePiece piece, bool isSacrifice = false)
    {
        string sacrificeText = (isSacrifice) ? "<color=#76428a> -Sacrifice- </color>" + Environment.NewLine: "";
        string evolvedText = (piece.evolved) ? "Evolved " : "";
        return
            $"<color=#76428a>{evolvedText}{piece.GetType().FullName}</color>{Environment.NewLine}{sacrificeText}<size=24>Level {piece.level}{Environment.NewLine}{Environment.NewLine}{piece.GetDescription()}</size>";
    }
}
