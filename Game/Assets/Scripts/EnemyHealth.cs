using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{

    public int health;
    public bool Shield = false;
    [SerializeField] private GameObject shield;

    public HitResponse Hit(float damage, int target, bool emp = false)
    {
        HitResponseBuilder hb = new HitResponseBuilder().Damage(damage).Target(target);
        if (target == 0) return hb.Build();
        if (emp && Shield) { Shield = false; Destroy(shield); return hb.Destroy().Build(); }
        if (!Shield && health <= damage)
        {
            Die();
            return hb.Destroy().Build();
        }
        return hb.Destroy().Build();
    }
    public void Die()
    {
        if (GetComponent<ItemDrop>() != null)
            GetComponent<ItemDrop>().DropItem();
        ScoreManager.instance?.AddPoints(100);
        Destroy(gameObject);
    }
}
