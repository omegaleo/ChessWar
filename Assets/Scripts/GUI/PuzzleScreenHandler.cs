using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleScreenHandler : InstancedBehaviour<PuzzleScreenHandler>
{
    [SerializeField] private GameObject panel;
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private TMP_Text prevButton;
    [SerializeField] private TMP_Text nextButton;
    
    private int currentPage = 0;
    
    public void Open()
    {
        panel.SetActive(true);
        pages[0].SetActive(true);
        currentPage = 0;
        prevButton.text = "<sprite=2>";
        nextButton.text = (pages.Count > 1) ? "<sprite=1>" : "<sprite=2>";
    }

    public void Next()
    {
        currentPage++;
        
        if (currentPage == pages.Count)
        {
            Close();
            return;
        }
        else if (currentPage == pages.Count - 1)
        {
            // Set next button to close
            nextButton.text = "<sprite=2>";
        }

        prevButton.text = "<sprite=0>";
        pages[currentPage].SetActive(true);
    }

    public void Previous()
    {
        currentPage--;
        
        if (currentPage < 0)
        {
            Close();
            return;
        }
        else if (currentPage == 0)
        {
            // Set previous button to close
            prevButton.text = "<sprite=2>";
        }

        nextButton.text = "<sprite=1>";
        pages[currentPage].SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
