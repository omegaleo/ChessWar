using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : InstancedBehaviour<OptionsMenu>
{
    [SerializeField] private GameObject panel;

    public void Open()
    {
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
