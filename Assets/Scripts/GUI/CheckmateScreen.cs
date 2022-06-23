using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckmateScreen : InstancedBehaviour<CheckmateScreen>
{
    [SerializeField] private GameObject panel;

    [SerializeField] private TMP_Text title;

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
