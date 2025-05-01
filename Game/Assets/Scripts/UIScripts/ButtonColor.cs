using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ButtonColor : MonoBehaviour
{
    private Selectable _button;
    
    private void Awake()
    {
        _button = GetComponent<Selectable>();
    }

    public ColorScheme.ColorOption selectedColor;
    public float multiplier = 1.0f;
    
    private void OnValidate()
    {
        if (_button == null)
        {
            _button = GetComponent<Selectable>();
        }
        UpdateColor();
    }

    public void UpdateColor()
    {
        Color color = ColorScheme.ColorMap[selectedColor].MultiplyRGB(multiplier);
        
        ColorBlock colors = new ColorBlock();
        colors.normalColor = color;
        colors.highlightedColor = color * 1.2f;
        colors.pressedColor = color.MultiplyRGB(0.5f);
        colors.selectedColor = color;
        colors.disabledColor = color * 0.2f;
        colors.colorMultiplier = 1;
        colors.fadeDuration = 0.2f;
        _button.colors = colors;
    } 
}