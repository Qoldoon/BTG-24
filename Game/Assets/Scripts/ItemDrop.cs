using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject Prefab;
    public string key;
    public void DropItem()
    {
        var item = Instantiate(Prefab, transform.position, Quaternion.identity);
        var component = item.gameObject.GetComponent<KeyCollect>();
        if (component != null)
            component.key = key;
    }
}
