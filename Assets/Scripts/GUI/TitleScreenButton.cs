using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenButton : Button
{
    public TMP_Text label;
    public string text;

    private int spriteStartPosition = 22;
    private int spriteEndPosition = 43;
    
    protected override void Start()
    {
        base.Start();
        label = GetComponent<TMP_Text>();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        var index = new System.Random().Next(spriteStartPosition, spriteEndPosition);
        label.text = $"<sprite={index}>{text}";
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        label.text = text;
    }
}
