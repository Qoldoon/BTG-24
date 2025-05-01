using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItems : MonoBehaviour
{
    public List<GameObject> selectedItems = new ();
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int Count => selectedItems.Count;

    public void TryAdd(GameObject item)
    {
        if (!selectedItems.Contains(item))
            selectedItems.Add(item);
        else selectedItems.Remove(item);
    }
}
