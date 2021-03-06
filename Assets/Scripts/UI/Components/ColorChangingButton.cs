﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChangingButton : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler
{
    public Button button;
    public Text text;

    public Sprite buttonHoverSprite;
    public Sprite buttonActiveSprite;

    public Color defaultTextColor;
    public Color selectedTextColor;

    private void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
    }

    public void Init(Action func)
    {
        SpriteState spriteState = new SpriteState();
        spriteState.highlightedSprite = buttonHoverSprite;
        spriteState.pressedSprite = buttonActiveSprite;

        button.transition = Selectable.Transition.SpriteSwap;
        button.spriteState = spriteState;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => func());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        text.color = selectedTextColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.color = defaultTextColor;
    }
}
