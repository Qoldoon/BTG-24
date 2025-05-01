using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SingleColor : MonoBehaviour
{
    private Graphic image;

    private void Awake()
    {
        image = GetComponent<Graphic>();
    }
    
    
    public ColorScheme.ColorOption selectedColor;
    public float multiplier = 1.0f;
    public float alpha = 1.0f;
    
    private void OnValidate()
    {
        if (image == null)
        {
            image = GetComponent<Graphic>();
        }
        UpdateColor();
    }

    public void UpdateColor()
    {
        Color color = ColorScheme.ColorMap[selectedColor];

        image.color = color.MultiplyRGB(multiplier).MultiplyAlpha(alpha);
    } 
}