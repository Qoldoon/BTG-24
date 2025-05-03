using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Door : MonoBehaviour
{
    Collider2D _collider;
    SpriteRenderer _renderer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }
    
    private List<GraphNode> doorNodes = new ();
    [SerializeField]
    private bool isOpen;
    private Color openColor;
    private Color closedColor;

    void Start()
    {
        openColor = _renderer.color * 0.3f;
        closedColor = _renderer.color;
        Bounds doorBounds = _collider.bounds;
        
        var gridGraph = AstarPath.active.data.gridGraph;
        
        doorNodes.Clear();
        doorNodes = gridGraph.GetNodesInRegion(doorBounds);
        
        if (isOpen) Open();
        else UpdateGraph();
    }
    
    public void Open()
    {
        _collider.enabled = false;
        _renderer.color = openColor;
        isOpen = true;
        UpdateGraph();
    }
    
    public void Close()
    {
        _collider.enabled = true;
        _renderer.color = closedColor;
        isOpen = false;
        UpdateGraph();
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        if (isOpen) Open();
        else Close();
    }
    private void UpdateGraph()
    {
        foreach (var node in doorNodes)
        {
            node.Walkable = isOpen;
        }
        
        var guo = new GraphUpdateObject
        {
            bounds = _collider.bounds,
            updatePhysics = false,
            modifyWalkability = true,
            setWalkability = isOpen
        };
        
        AstarPath.active.UpdateGraphs(guo);
    }
}
