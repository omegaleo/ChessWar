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
        rightBtn.text = "<sprite=1>";
        if (currentPage < 0)
        {
            panel.SetActive(false);
            return;
        }
        else if (currentPage == 0)
        {
            leftBtn.text = "<sprite=2>";
        }
        else
        {
            leftBtn.text = "<sprite=0>";
        }
        SetActivePage(currentPage);
    }

    public void NextPage()
    {
        currentPage++;
        leftBtn.text = "<sprite=0>";
        if (currentPage >= pages.Count)
        {
            panel.SetActive(false);
            return;
        }
        else if (currentPage == pages.Count - 1)
        {
            rightBtn.text = "<sprite=2>";
        }
        else
        {
            rightBtn.text = "<sprite=1>";
        }
        SetActivePage(currentPage);
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
