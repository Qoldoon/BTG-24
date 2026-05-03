using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public enum DoorTriggerMode { Toggle, OpenOnly, CloseOnly }

public class Door : MonoBehaviour
{
    Collider2D _collider;
    BoxCollider2D _trigger;
    SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            if (col.isTrigger) _trigger = col as BoxCollider2D;
            else               _collider = col;
        }

        if (_collider == null) Debug.LogWarning("Door: no non-trigger Collider2D found.");
        if (_trigger  == null) Debug.LogWarning("Door: no BoxCollider2D trigger found.");
    }

    public void SetTriggerShape(Vector2 offset, Vector2 size)
    {
        if (_trigger == null)
        {
            Debug.LogWarning("Door: SetTriggerShape called but no BoxCollider2D trigger found.");
            return;
        }
        _trigger.offset = offset;
        _trigger.size   = size;
    }

    private List<GraphNode> doorNodes = new ();

    [SerializeField] private bool isOpen;
    [SerializeField] public DoorTriggerMode triggerMode = DoorTriggerMode.Toggle;

    private Color openColor;
    private Color closedColor;

    // Called by LevelLoader before Start() to override inspector defaults.
    public void InitFromJson(bool startOpen, DoorTriggerMode mode)
    {
        isOpen      = startOpen;
        triggerMode = mode;
    }

    void Start()
    {
        openColor   = _renderer.color * 0.3f;
        closedColor = _renderer.color;

        var gridGraph = AstarPath.active.data.gridGraph;

        // Expand query bounds so a thin or rotated collider doesn't miss the gap node.
        var queryBounds = _collider.bounds;
        queryBounds.Expand(1f);

        doorNodes.Clear();
        doorNodes = gridGraph.GetNodesInRegion(queryBounds);

        if (isOpen) Open();
        else UpdateGraph();
    }

    public void Open()
    {
        _collider.enabled = false;
        _renderer.color   = openColor;
        isOpen = true;
        UpdateGraph();
    }

    public void Close()
    {
        _collider.enabled = true;
        _renderer.color   = closedColor;
        isOpen = false;
        UpdateGraph();
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        if (isOpen) Open();
        else Close();
    }

    /// <summary>
    /// Called by the door's built-in trigger (replace Toggle() with Activate() in the prefab's UnityEvent).
    /// Behaviour depends on triggerMode set in the Inspector or overridden by JSON.
    /// </summary>
    public void Activate()
    {
        switch (triggerMode)
        {
            case DoorTriggerMode.OpenOnly:  Open();   break;
            case DoorTriggerMode.CloseOnly: Close();  break;
            default:                        Toggle(); break;
        }
    }
    private void UpdateGraph()
    {
        foreach (var node in doorNodes)
            node.Walkable = isOpen;

        // Expand bounds by one node-width so neighbouring nodes outside the door
        // are included in the recalculation — without this, their connections back
        // to the door node aren't restored when the door opens.
        var bounds = _collider.bounds;
        bounds.Expand(2f);

        var guo = new GraphUpdateObject
        {
            bounds            = bounds,
            updatePhysics     = false,
            modifyWalkability = false,
        };

        AstarPath.active.UpdateGraphs(guo);
    }
}
