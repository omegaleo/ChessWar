using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsMenu : InstancedBehaviour<OptionsMenu>
{
    [SerializeField] private GameObject panel;

    [SerializeField] private TMP_Dropdown difficultyDropdown;
    
    public void Open()
    {
        panel.SetActive(true);
        difficultyDropdown.value = (int) GameManager.instance.difficulty;
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void SetDifficulty()
    {
        GameManager.instance.difficulty = (GameManager.Difficulty) difficultyDropdown.value;
    }
}
