using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButton : Button
{
    public TMP_Text title;
    public Image image;
    public Puzzle puzzle;
    public Sprite comingSoonSprite;

    protected override void Awake()
    {
        base.Awake();
    }

    [ExecuteInEditMode]
    public void Update()
    {
        if (Application.isPlaying)
        {
            return; // Code below should not be executed in Play Mode
        }

        if (title != null && image != null)
        {
            if (puzzle == null)
            {
                title.text = "Coming soon!";
                image.enabled = false;
            }
            else
            {
                title.text = puzzle.puzzleName;
                image.sprite = puzzle.image;
                image.preserveAspect = true;
            }
        }
    }

    public void PlayPuzzle()
    {
        if (puzzle == null)
        {
            return;
        }
        
        GameManager.instance.StartPuzzle(puzzle);
    }
}