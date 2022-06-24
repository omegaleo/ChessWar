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

        currentDescription.text = $"<color=#76428a>{nameof(piece)}</color>{Environment.NewLine}<size=24>Level {piece.level}</size>";
    }

    public void CloseCurrent()
    {
        currentPanel.SetActive(false);
    }
    
    public void OpenTarget(BasePiece piece)
    {
        targetPanel.SetActive(true);
        
        targetIcon.sprite = (piece.color == Color.black) ? 
            (piece.evolved) ? piece.eBlackSprite : piece.blackSprite :
            (piece.evolved) ? piece.eWhiteSprite : piece.whiteSprite;

        targetDescription.text = $"<color=#76428a>{nameof(piece)}</color>{Environment.NewLine}<size=24>Level {piece.level}</size>";
    }

    public void CloseTarget()
    {
        targetPanel.SetActive(false);
    }
}
