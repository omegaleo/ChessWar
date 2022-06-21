using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckmateScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private TMP_Text title;

    public static CheckmateScreen instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Open(string text)
    {
        panel.SetActive(true);
        title.text = text;
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
