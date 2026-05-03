using System;

[Serializable]
public class LevelData
{
    public string name;
    public int levelIndex;
    
    public int width    = 22;
    public int height   = 17;
    public int originX  = 0;
    public int originY  = 0;

    public string wallColorHex = "#3d3d3d";

    public LevelEntity[] entities;
}

[Serializable]
public class LevelEntity
{
    public string type;
    public float x;
    public float y;

    // "N" (default) | "E" | "S" | "W"
    public string dir;
    
    public bool open;
    
    // "toggle" (default) | "open" | "close"
    public string mode;
    
    public float triggerOffsetX;
    public float triggerOffsetY;
    public float triggerWidth;
    public float triggerHeight;
}
