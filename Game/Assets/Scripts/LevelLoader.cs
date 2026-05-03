using System.IO;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLoader : MonoBehaviour
{
    [Header("Level File")]
    public string levelFileName;

    [Header("Base Scene Objects")]
    public GameObject pathfindingPrefab;
    public int bgPadding = 20;

    [Header("Characters")]
    public GameObject playerSpawnPrefab;
    public GameObject enemyPrefab;
    public GameObject shieldedEnemyPrefab;
    public GameObject armoredEnemyPrefab;
    public GameObject keyEnemyPrefab;

    [Header("Environment")]
    public GameObject doorPrefab;
    public GameObject exitPrefab;
    public GameObject keyItemPrefab;

    [Header("Pickup Container")]
    public GameObject pickupPrefab;

    [Header("Weapons & Items")]
    public GameObject blasterItem;
    public GameObject minigunItem;
    public GameObject sniperItem;
    public GameObject launcherItem;
    public GameObject grenadeItem;
    public GameObject ammoPouchItem;

    [Header("Power-ups")]
    public GameObject powerAmpPrefab;
    public GameObject speedPrefab;
    public GameObject hpPrefab;
    public GameObject ammoRefillPrefab;
    
    private TileBase _wallTile;
    private TileBase _bgWallTile;

    void Awake()
    {
        if (!string.IsNullOrEmpty(LevelSession.levelFile))
            levelFileName = LevelSession.levelFile;

        if (string.IsNullOrEmpty(levelFileName)) return;

        string path = System.IO.Path.Combine(Application.streamingAssetsPath, levelFileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"LevelLoader: file not found: {path}");
            return;
        }

        _wallTile   = Resources.Load<TileBase>("WallTiles");
        _bgWallTile = Resources.Load<TileBase>("WallTiles_BG");
        if (_wallTile   == null) Debug.LogWarning("LevelLoader: WallTiles not found in Resources/");
        if (_bgWallTile == null) Debug.LogWarning("LevelLoader: WallTiles_BG not found in Resources/");

        LevelData data = JsonUtility.FromJson<LevelData>(File.ReadAllText(path));
        
        if (pathfindingPrefab != null)
            Instantiate(pathfindingPrefab);
        else
            Debug.LogWarning("LevelLoader: pathfindingPrefab not assigned — enemies won't have A*.");

        BuildFloor(data);
        BuildWalls(data);
        
        if (AstarPath.active != null)
            AstarPath.active.Scan();
        else
            Debug.LogWarning("LevelLoader: AstarPath not active — enemies may ignore walls.");

        SpawnEntities(data);
    }
    
    // Floor
    void BuildFloor(LevelData data)
    {
        var sprite = Resources.Load<Sprite>("Textures/Floor");
        if (sprite == null)
        {
            Debug.LogWarning("LevelLoader: Floor sprite not found at Resources/Textures/Floor");
            return;
        }

        var go = new GameObject("Floor");
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite            = sprite;
        sr.drawMode          = SpriteDrawMode.Tiled;
        sr.size              = new Vector2(data.width - 2, data.height - 2);
        sr.sortingLayerName  = "Background";
        sr.sortingOrder      = -1;
        
        go.transform.position = new Vector3(
            data.originX + data.width  * 0.5f,
            data.originY + data.height * 0.5f,
            0f);
    }
    
    // Walls
    void BuildWalls(LevelData data)
    {
        var gridGO = new GameObject("Grid");
        gridGO.AddComponent<Grid>();
        
        var wallMap = MakeTilemap(gridGO, "walls", "Default", 0);

        if (_wallTile != null)
        {
            if (_bgWallTile != null)
                PlaceBgFill(wallMap, data);
            
            PlaceBorderRing(wallMap, data);
            
            foreach (var e in data.entities)
                if (e.type == "wall")
                    wallMap.SetTile(Cell(e), _wallTile);
        }
        
        Color wallColor = new Color(0.239f, 0.239f, 0.239f); // #3d3d3d
        if (!string.IsNullOrEmpty(data.wallColorHex))
            ColorUtility.TryParseHtmlString(data.wallColorHex, out wallColor);
        wallMap.color = wallColor;
    }

    Tilemap MakeTilemap(GameObject gridParent, string goName, string sortingLayer, int sortingOrder)
    {
        var go = new GameObject(goName);
        go.transform.SetParent(gridParent.transform, false);
        var tm = go.AddComponent<Tilemap>();
        var tr = go.AddComponent<TilemapRenderer>();
        tr.sortingLayerName = sortingLayer;
        tr.sortingOrder     = sortingOrder;
        return tm;
    }

    void PlaceBgFill(Tilemap tm, LevelData data)
    {
        int ringLeft  = data.originX;
        int ringRight = data.originX + data.width  - 1;
        int ringBot   = data.originY;
        int ringTop   = data.originY + data.height - 1;

        int fillLeft  = ringLeft  - bgPadding;
        int fillRight = ringRight + bgPadding;
        int fillBot   = ringBot   - bgPadding;
        int fillTop   = ringTop   + bgPadding;

        for (int x = fillLeft; x <= fillRight; x++)
        for (int y = fillBot;  y <= fillTop;   y++)
        {
            if (x > ringLeft && x < ringRight && y > ringBot && y < ringTop)
                continue;

            tm.SetTile(new Vector3Int(x, y, 0), _bgWallTile);
        }
    }

    void PlaceBorderRing(Tilemap tm, LevelData data)
    {
        int left  = data.originX;
        int right = data.originX + data.width  - 1;
        int bot   = data.originY;
        int top   = data.originY + data.height - 1;

        for (int x = left; x <= right; x++)
        {
            tm.SetTile(new Vector3Int(x, bot, 0), _wallTile);
            tm.SetTile(new Vector3Int(x, top, 0), _wallTile);
        }
        for (int y = bot + 1; y < top; y++)
        {
            tm.SetTile(new Vector3Int(left,  y, 0), _wallTile);
            tm.SetTile(new Vector3Int(right, y, 0), _wallTile);
        }
    }

    void SpawnEntities(LevelData data)
    {
        foreach (var entity in data.entities)
        {
            if (entity.type == "wall") continue;

            // Tilemap cells are bottom-left aligned; shift to cell center.
            var pos = new Vector3(entity.x + 0.5f, entity.y + 0.5f, 0f);

            if (entity.type.StartsWith("pickup_"))
            {
                SpawnPickup(entity, pos);
                continue;
            }

            var prefab = PrefabFor(entity.type);
            if (prefab == null)
            {
                Debug.LogWarning($"LevelLoader: unknown entity type '{entity.type}'");
                continue;
            }

            var go = Instantiate(prefab, pos, DirToRotation(entity.dir));

            if (entity.type == "exit" && go.TryGetComponent<LevelExit>(out var exit))
            {
                exit.level = data.levelIndex;
                if (go.TryGetComponent<ScoreManager>(out var sm))
                    sm.level = data.levelIndex;
            }

            if (entity.type == "door" && go.TryGetComponent<Door>(out var door))
            {
                door.InitFromJson(entity.open, ParseTriggerMode(entity.mode));
                if (entity.triggerWidth != 0 || entity.triggerHeight != 0)
                    door.SetTriggerShape(
                        new Vector2(entity.triggerOffsetX, entity.triggerOffsetY),
                        new Vector2(entity.triggerWidth,   entity.triggerHeight));
            }
        }
    }

    void SpawnPickup(LevelEntity entity, Vector3 pos)
    {
        if (pickupPrefab == null)
        {
            Debug.LogWarning("LevelLoader: pickupPrefab not assigned.");
            return;
        }

        var item = ItemFor(entity.type);
        if (item == null)
        {
            Debug.LogWarning($"LevelLoader: no item prefab assigned for '{entity.type}'");
            return;
        }

        var go = Instantiate(pickupPrefab, pos, Quaternion.identity);
        go.GetComponent<Pickup>().item = item;
    }

    static Vector3Int Cell(LevelEntity e) =>
        new Vector3Int(Mathf.RoundToInt(e.x), Mathf.RoundToInt(e.y), 0);

    GameObject PrefabFor(string type) => type switch
    {
        "playerSpawn"      => playerSpawnPrefab,
        "enemy"            => enemyPrefab,
        "enemy_shielded"   => shieldedEnemyPrefab,
        "enemy_armored"    => armoredEnemyPrefab,
        "enemy_key"        => keyEnemyPrefab,
        "door"             => doorPrefab,
        "exit"             => exitPrefab,
        "key"              => keyItemPrefab,
        "powerup_amp"      => powerAmpPrefab,
        "powerup_speed"    => speedPrefab,
        "powerup_hp"       => hpPrefab,
        "powerup_ammo"     => ammoRefillPrefab,
        _                  => null
    };
    
    static Quaternion DirToRotation(string dir) => dir switch
    {
        "E" => Quaternion.Euler(0f, 0f, -90f),
        "S" => Quaternion.Euler(0f, 0f,  180f),
        "W" => Quaternion.Euler(0f, 0f,   90f),
        _   => Quaternion.identity
    };

    static DoorTriggerMode ParseTriggerMode(string mode) => mode switch
    {
        "open"  => DoorTriggerMode.OpenOnly,
        "close" => DoorTriggerMode.CloseOnly,
        _       => DoorTriggerMode.Toggle
    };
    
    GameObject ItemFor(string type) => type switch
    {
        "pickup_blaster"   => blasterItem,
        "pickup_minigun"   => minigunItem,
        "pickup_sniper"    => sniperItem,
        "pickup_launcher"  => launcherItem,
        "pickup_grenade"   => grenadeItem,
        "pickup_ammoPouch" => ammoPouchItem,
        _                  => null
    };
}
