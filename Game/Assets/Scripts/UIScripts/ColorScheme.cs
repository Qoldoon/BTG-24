using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class ColorScheme
{
    public static Color PrimaryColor = HexToRGB("#8e44ad");
    public static Color SecondaryColor = HexToRGB("#16a085");
    public static Color TertiaryColor = HexToRGB("#2c3e50");

    public static Color PrimaryAccentColor = HexToRGB("#c0392b");
    public static Color SecondaryAccentColor = HexToRGB("#ecf0f1");

    public enum ColorOption
    {
        Primary,
        Secondary,
        Tertiary,
        Accent,
        White
    }
    
    public static readonly Dictionary<ColorOption, Color> ColorMap = new()
    {
        { ColorOption.Primary, PrimaryColor },
        { ColorOption.Secondary, SecondaryColor },
        { ColorOption.Tertiary, TertiaryColor },
        { ColorOption.Accent, PrimaryAccentColor },
        { ColorOption.White, SecondaryAccentColor }
    };
    
    public static Color HexToRGB(string hex)
    {
        hex = hex.Replace("#", "");
        hex = hex.Replace(" ", "");
        int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        return new Color(r/255f, g/255f, b/255f);
    }
    
    public static Color MultiplyRGB(this Color color, float multiplier)
    {
        return new Color(
            Mathf.Clamp01(color.r * multiplier),
            Mathf.Clamp01(color.g * multiplier),
            Mathf.Clamp01(color.b * multiplier),
            color.a
        );
    }
    
    public static Color MultiplyAlpha(this Color color, float multiplier)
    {
        return new Color(
            color.r,
            color.g,
            color.b,
            Mathf.Clamp01(color.a * multiplier)
        );
    }
}