using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BorderCell : MonoBehaviour
{
    public TMP_Text label;

    public void Set(string text)
    {
        label.text = text;
    }
}
