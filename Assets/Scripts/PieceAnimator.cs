using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class PieceAnimator : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float disappearValue;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        
        if (image.material != null)
        {
            image.material.SetFloat("_DissolveAmount", disappearValue);
        }
    }
}
