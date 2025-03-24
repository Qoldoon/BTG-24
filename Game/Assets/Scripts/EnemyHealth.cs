using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{

    public int health;
    public bool Shield = false;

    public HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false)
    {
        HitResponseBuilder hb = new HitResponseBuilder().Damage(damage).Target(target);
        if (target == 0) return hb.Build();
        if (emp) { Shield = false; return hb.Destroy().Build(); }
        if (!Shield && health <= damage)
        {
            Die();
            return hb.Destroy().Build();
        }
        return hb.Build();
    }
    public void Die()
    {
        if (GetComponent<ItemDrop>() != null)
            GetComponent<ItemDrop>().DropItem();
        //ScoreManager.instance.AddPointForMeleeKill(gameObject.tag);
        Destroy(gameObject);
    }
}
