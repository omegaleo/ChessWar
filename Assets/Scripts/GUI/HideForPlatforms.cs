using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideForPlatforms : MonoBehaviour
{
    [SerializeField] private List<RuntimePlatform> platformsToHideButton;

    private void Start()
    {
        if (platformsToHideButton.Contains(Application.platform))
        {
            this.gameObject.SetActive(false);
        }
    }
}
