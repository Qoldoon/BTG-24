using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class ColorScheme
{
    public static Color PrimaryColor = HexToRGB("#34b4d6");
    public static Color SecondaryColor = HexToRGB("#3f8125");
    public static Color TertiaryColor = HexToRGB("#1d2028");

    public static Color PrimaryAccentColor = HexToRGB("#a8463f");
    public static Color SecondaryAccentColor = HexToRGB("#ececec");

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