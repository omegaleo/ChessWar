using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PieceSprite
{
    public string pieceIdentifier;
    public Sprite whiteSprite;
    public Sprite blackSprite;
    
    public Sprite eWhiteSprite;
    public Sprite eBlackSprite;

    /// <summary>
    /// List of sprites for the animation of taking an enemy piece
    /// </summary>
    public List<Sprite> AttackAnimation;
    
    /// <summary>
    /// List of sprites for the animation of claiming an ally piece as a sacrifice
    /// </summary>
    public List<Sprite> ClaimSacrificeAnimation;

    /// <summary>
    /// List of sprites for the animation of being claimed as a sacrifice
    /// </summary>
    public List<Sprite> SacrificeAnimation;
}