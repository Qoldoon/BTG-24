using UnityEngine;

public static class LevelPreviewGenerator
{
    const int TargetW = 160;
    const int TargetH = 100;

    static readonly Color FloorColor  = new Color(0.42f, 0.42f, 0.46f, 1f);
    static readonly Color PlayerColor = new Color(0.55f, 0.55f, 0.95f, 1f);
    static readonly Color ExitColor   = new Color(0.80f, 0.55f, 0.20f, 1f);
    static readonly Color EnemyColor  = new Color(0.90f, 0.20f, 0.20f, 1f);
    static readonly Color KeyColor    = new Color(1.00f, 0.85f, 0.10f, 1f);
    static readonly Color DoorColor   = new Color(0.80f, 0.55f, 0.20f, 1f);
    static readonly Color PickupColor = new Color(0.20f, 0.90f, 0.30f, 1f);

    public static Sprite Generate(LevelData data)
    {
        if (!ColorUtility.TryParseHtmlString(data.wallColorHex, out Color wallColor))
            wallColor = new Color(0.23f, 0.23f, 0.36f, 1f);
        
        int gridW = data.width;
        int gridH = data.height;

        float cellW = (float)TargetW / gridW;
        float cellH = (float)TargetH / gridH;

        var pixels = new Color[TargetW * TargetH];
        
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = wallColor;
        
        for (int cy = 1; cy <= gridH - 2; cy++)
        for (int cx = 1; cx <= gridW - 2; cx++)
            FillCell(pixels, cx, cy, cellW, cellH, FloorColor);
        
        if (data.entities != null)
        {
            foreach (var e in data.entities)
            {
                if ((e.type ?? "") != "wall") continue;
                int cx = Mathf.RoundToInt(e.x - data.originX);
                int cy = Mathf.RoundToInt(e.y - data.originY);
                if (cx < 0 || cx >= gridW || cy < 0 || cy >= gridH) continue;
                FillCell(pixels, cx, cy, cellW, cellH, wallColor);
            }

            foreach (var e in data.entities)
            {
                var t = e.type ?? "";
                if (t == "wall") continue;

                int cx = Mathf.RoundToInt(e.x - data.originX);
                int cy = Mathf.RoundToInt(e.y - data.originY);
                if (cx < 0 || cx >= gridW || cy < 0 || cy >= gridH) continue;

                Color c;
                if      (t == "door")                                          c = DoorColor;
                else if (t == "playerSpawn")                                   c = PlayerColor;
                else if (t == "exit")                                          c = ExitColor;
                else if (t == "key")                                           c = KeyColor;
                else if (t == "enemy"        || t == "enemy_shielded" ||
                         t == "enemy_armored" || t == "enemy_key")             c = EnemyColor;
                else if (t.StartsWith("pickup_") || t.StartsWith("powerup_")) c = PickupColor;
                else continue;

                FillCell(pixels, cx, cy, cellW, cellH, c);
            }
        }

        var tex = new Texture2D(TargetW, TargetH, TextureFormat.RGB24, false);
        tex.filterMode = FilterMode.Point;
        tex.SetPixels(pixels);
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, TargetW, TargetH), new Vector2(0.5f, 0.5f), 1f);
    }

    static void FillCell(Color[] pixels, int cx, int cy, float cellW, float cellH, Color c)
    {
        int x0 = Mathf.RoundToInt(cx * cellW);
        int y0 = Mathf.RoundToInt(cy * cellH);
        int x1 = Mathf.RoundToInt((cx + 1) * cellW);
        int y1 = Mathf.RoundToInt((cy + 1) * cellH);

        for (int py = y0; py < y1 && py < TargetH; py++)
        for (int px = x0; px < x1 && px < TargetW; px++)
            pixels[py * TargetW + px] = c;
    }
}
