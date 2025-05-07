using System;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Boundary : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;

    private int lastWidth = -1;
    private int lastHeight = -1;

    private void Start()
    {
        Update();
    }

    void Update()
    {
        if (lastWidth != width || lastHeight != height)
        {
            RebuildWall();
            lastWidth = width;
            lastHeight = height;
        }
    }

    // private void LateUpdate()
    // {
    //     foreach(Transform block in transform)
    //     {
    //         block.GetComponent<BoxCollider2D>().compositeOperation = Collider2D.CompositeOperation.Merge;
    //     }
    // }

    void RebuildWall()
    {
        List<Transform> list = new();
        foreach (Transform child in transform)
        {
            list.Add(child);
        }
        foreach (var child in list)
            DestroyImmediate(child.gameObject);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x == 0 || x == width-1 || y == 0 || y == height-1)
                {
                    Vector3 blockPosition = new Vector3(x, y, 0);

                    GameObject block = Instantiate(wall, transform);
                    block.transform.localPosition = blockPosition;
                    block.tag = "Untagged";
                    DestroyImmediate(block.GetComponent<WallState>());
                }
            }
        }
    }
    
    private void OnValidate()
    {
        width = Mathf.Max(0, width);
        height = Mathf.Max(0, height);
    }
}
