using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject Prefab;
    public void DropItem()
    {
        Instantiate(Prefab, transform.position, Quaternion.identity);
    }
}
