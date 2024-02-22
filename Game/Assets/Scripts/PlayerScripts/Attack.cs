using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.Mouse0)) return;
        var weapon = PlayerInventory.items[GetComponentInChildren<PlayerInventory>().current];
        if (!weapon.isGun && !weapon.isGrenade) return;
        if (weapon.isGun) weapon.go.GetComponent<Shoot>().Attack();
        if (weapon.isGrenade) weapon.go.GetComponent<Throw>().Attack();

    }
}
