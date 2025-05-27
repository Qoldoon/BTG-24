using System;
using UnityEngine;

public class BlockColor : MonoBehaviour
{
    public Sprite[] sprites;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Texture2D atlasTexture;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(atlasTexture == null) return;
        sprites = Resources.LoadAll<Sprite>(atlasTexture.name);
    }

    private void Start()
    {
        Invoke(nameof(UpdateSprite), 0.02f);
    }

    void UpdateSprite()
    {
        if(sprites.Length == 0) return;
        int neighborConfig = GetNeighborConfiguration();
        try
        {
            _spriteRenderer.sprite = sprites[neighborConfig];

        }
        catch (Exception e)
        {
            Debug.LogError($"{e}, {name}");
            Debug.Log($"{name}, got number {neighborConfig}");
        }
    }
    
    public void OnBlockPlaced()
    {
        UpdateSprite();
        foreach (Vector2Int offset in new Vector2Int[] { new(0, 1), new(0, -1), new(-1, 0), new(1, 0) })
        {
            Vector3 checkPos = transform.position + new Vector3(offset.x, offset.y, 0);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPos, 0.1f);
            foreach (var col in colliders)
            {
                var neighbor = col.GetComponent<BlockColor>();
                if (neighbor != null)
                    neighbor.UpdateSprite();
            }
        }
    }
    
    int GetNeighborConfiguration()
    {
        int config = 0;
        Vector2Int[] neighborOffsets = {
            new (0, 1),  // Top
            new (0, -1), // Bottom
            new (-1, 0), // Left
            new (1, 0),   // Right
            new (-1, 1),  // Top Left
            new (1, 1), // Top Right
            new (-1, -1), // Bottom Left
            new (1, -1)   // Bottom Right
        };

        for (int i = 0; i < neighborOffsets.Length; i++)
        {
            Vector3 checkPos = transform.position + new Vector3(neighborOffsets[i].x, neighborOffsets[i].y, 0);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPos, 0.1f);
            foreach (var col in colliders)
            {
                if (col.gameObject != gameObject && col.GetComponent<BlockColor>())
                {
                    config |= (1 << i);
                }
            }
        }
        return Validate(config);
    }

    private static int Validate(int config)
    {
        var end = config & 15;
        var start = (config & 240) / 16;
        
        // All four
        if (end == 0)
        {
            return end;
        }
        
        // U-shape
        if (end is 1 or 2 or 4 or 8)
            return end;
        // Parallel
        if(end is 12 or 3)
            return end;
        // BR Corner
        if (end is 5 && (start & 1) is 0)
            return 16;
        if (end is 5)
            return end;
        // TR Corner
        if (end is 6 && (start & 4) is 0)
            return 17;
        if (end is 6)
            return end;
        // BL Corner
        if (end is 9 && (start & 2) is 0)
            return 18;
        if(end is 9)
            return end;
        // TL Corner
        if (end is 10 && (start & 8) is 0)
            return 19;
        if (end is 10)
            return end;
        // Right Side
        if (end is 7)
        {
            if (start is 5 or 7 or 13 or 15)
                return end;
            if ((start & 1) is 0 && (start & 4) is 0)
                return 22;
            if ((start & 1) is 0)
                return 21;
            if ((start & 4) is 0)
                return 20;
        }
        // Left Side
        if (end is 11)
        {
            if (start is 10 or 11 or 14 or 15)
                return end;
            if ((start & 2) is 0 && (start & 8) is 0)
                return 25;
            if ((start & 2) is 0)
                return 24;
            if ((start & 8) is 0)
                return 23;
        }
        // Bottom Side
        if (end is 13)
        {
            if (start is 3 or 7 or 11 or 15) 
                return end;
            if ((start & 1) is 0 && (start & 2) is 0)
                return 28;
            if ((start & 1) is 0)
                return 27;
            if ((start & 2) is 0)
                return 26;
        }
        // Top Side
        if (end is 14)
        {
            if (start is 12 or 13 or 14 or 15)
                return end;
            if ((start & 4) is 0 && (start & 8) is 0)
                return 31;
            if ((start & 4) is 0)
                return 30;
            if ((start & 8) is 0)
                return 29;
        }
        
        // Blank
        if (end is 15)
        {
            if (start is 15)
                return end;
            
            // Four
            if (start is 0)
                return 46;
            
            // Three
            if (start is 1)
                return 42;
            if (start is 2)
                return 43;
            if (start is 4)
                return 44;
            if (start is 8)
                return 45;
            
            // Two
            if (start is 3)
                return 36;
            if (start is 5)
                return 37;
            if (start is 10)
                return 38;
            if (start is 12)
                return 39;
            if (start is 6)
                return 40;
            if (start is 9)
                return 41;
            // One
            if (start is 14)
                return 32;
            if (start is 13)
                return 33;
            if (start is 11)
                return 34;
            if (start is 7)
                return 35;
        }
        return config;
    }
}