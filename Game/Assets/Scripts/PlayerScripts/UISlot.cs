using System;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    [SerializeField]
    private Color colorA;
    [SerializeField]
    private Color colorB;
    public RawImage image;
    public Image icon;
    void Start()
    {
        image = GetComponent<RawImage>();
        icon = GetComponentInChildren<Image>();
    }

    public void Toggle()
    {
        image.color = image.color == colorA ? colorB : colorA;
    }

    public void ToggleOn()
    {
        image.color = colorB;
    }
    
    public void ToggleOff()
    {
        image.color = colorA;
    }
}
