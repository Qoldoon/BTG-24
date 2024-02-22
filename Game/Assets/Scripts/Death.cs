using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public void Die()
    {
        if (GetComponent<ItemDrop>() != null)
            GetComponent<ItemDrop>().DropItem();
        Destroy(gameObject);
    }
}
