using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class DynamicWall : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;

    private int lastWidth = -1;
    private int lastHeight = -1;

    void Update()
    {
        if (lastWidth != width || lastHeight != height)
        {
            RebuildWall();
            lastWidth = width;
            lastHeight = height;
        }
    }

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
                Vector3 blockPosition = new Vector3(x, y, 0);
                
                GameObject block = Instantiate(wall, transform);
                block.transform.localPosition = blockPosition;
                block.hideFlags = HideFlags.NotEditable;
                block.hideFlags = HideFlags.HideInHierarchy;
            }
        }
    }
    
    private void OnValidate()
    {
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
    }
}