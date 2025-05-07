using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRefill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var inv = collision.GetComponent<PlayerInventory>();
            if(inv == null) return;
            if(!inv.IsUsable(out var item)) return;
            if(item is not Weapon weapon) return;
            if(!weapon.NeedsReload()) return;
            weapon.SecondaryUse();
            inv.canvas.CreateText("Ammo");
            Destroy(gameObject);
        }
    }
}
