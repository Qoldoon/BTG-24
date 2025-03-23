using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{

    public int health;
    public bool Shield = false;

    public bool Hit(float damage, int target, bool emp = false)
    {
        if (target != 1) return false;
        if (emp) { Shield = false; return true; }
        if (!Shield && health <= damage)
        {
            Die();
            return true;
        }
        return false;
    }
    public void Die()
    {
        if (GetComponent<ItemDrop>() != null)
            GetComponent<ItemDrop>().DropItem();
        //ScoreManager.instance.AddPointForMeleeKill(gameObject.tag);
        Destroy(gameObject);
    }
}
