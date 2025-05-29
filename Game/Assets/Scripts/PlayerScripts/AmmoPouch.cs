using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class AmmoPouch : Item
{
    [SerializeField] private int reloads = 60;
    public Sprite sprite;
    
    public override string Description { get; set; } =
        "Spare ammunition \n" +
        "Allows reloading for all weapons \n";
    public override void OnAdd(PlayerInventory inventory, int index)
    {
        base.OnAdd(inventory, index);
        transform.localPosition = new Vector3(0f, 0f, 0f);
        PlayerInventory.reloads = reloads;
        
        CreateBox("Box 1", new Vector3(0, -0.4f, 0), Quaternion.Euler(0, 0, 0));
        CreateBox("Box 2", new Vector3(-0.3f, -0.37f, 0), Quaternion.Euler(0, 0, -15));
        CreateBox("Box 3", new Vector3(0.3f, -0.37f, 0), Quaternion.Euler(0, 0, 15));
        
        PlayerInventory.Reload(0);
    }

    private void CreateBox(string s, Vector3 position, Quaternion rotation)
    {
        GameObject box = new GameObject(s);
        var renderer = box.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = ColorScheme.HexToRGB("3D3D3D");
        renderer.sortingOrder = 2;
        renderer.sortingLayerName = "Game";
        box.transform.SetParent(transform.parent);
        box.transform.localPosition = position;
        box.transform.localRotation = rotation;
        box.transform.localScale = new  Vector3(0.25f, 0.2f, 1);
        PlayerInventory.Boxes.Add(box);
    }

    public override void OnRemove(int index)
    { 
        PlayerInventory.Boxes.ForEach(Destroy);
        PlayerInventory.Boxes.Clear();
        reloads = PlayerInventory.reloads;
        PlayerInventory.reloads = 0;
        PlayerInventory.canReload = false;
        base.OnRemove(index);
    }
}