using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItems : MonoBehaviour
{
    public static List<GameObject> selectedItems;
    public bool add = true;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        selectedItems = new List<GameObject>();
    }

    public int count
    {
        get
        {
            return selectedItems.Count;
        }
    }
    public void changeBehaviour(bool intput)
    {
        add = intput;
    }
    public void selectItem(GameObject item)
    {
        if(!add) { unSelectItem(item); return; }
        selectedItems.Add(item);
    }
    public void unSelectItem(GameObject item)
    {
        selectedItems.Remove(item);
    }
}
