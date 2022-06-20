using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HowToPlayScreen : MonoBehaviour
{
    [SerializeField] private int currentPage = 0;
    [SerializeField] private GameObject panel;
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private TMP_Text leftBtn;
    [SerializeField] private TMP_Text rightBtn;
    
    public static HowToPlayScreen instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PreviousPage()
    {
        currentPage--;
        
        if (currentPage < 0)
        {
            panel.SetActive(false);
        }
        else if (currentPage == 0)
        {
            leftBtn.text = "<sprite=2>";
        }
        else
        {
            rightBtn.text = "<sprite=0>";
        }
    }

    public void NextPage()
    {
        currentPage++;
        
        if (currentPage >= pages.Count)
        {
            panel.SetActive(true);
        }
        else if (currentPage == pages.Count - 1)
        {
            rightBtn.text = "<sprite=2>";
        }
        else
        {
            rightBtn.text = "<sprite=1>";
        }
    }
    
    public void Open()
    {
        currentPage = 0;
        rightBtn.text = "<sprite=1>";
        leftBtn.text = "<sprite=2>";
        SetActivePage(currentPage);
        panel.SetActive(true);
    }

    private void SetActivePage(int page)
    {
        pages.ForEach(p =>
        {
            p.SetActive(false);
        });
        
        pages[page].SetActive(true);
    }
}
