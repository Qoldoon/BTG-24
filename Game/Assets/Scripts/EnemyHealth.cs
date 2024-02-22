using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int health;
    public bool Shield = false;

    public void Hit(int damage, bool emp = false)
    {
        if (emp) { Shield = false; return; }
        if (!Shield && health <= damage)
        {
            Die();
        }
    }
    public void Die()
    {
        if (GetComponent<ItemDrop>() != null)
            GetComponent<ItemDrop>().DropItem();
        ScoreManager.instance.AddPointForMeleeKill(gameObject.tag);
        Destroy(gameObject);
    }
}
