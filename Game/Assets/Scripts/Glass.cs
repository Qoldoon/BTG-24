using UnityEngine;

public class Glass : MonoBehaviour, IDamageable
{
    public HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false, float radius = 0)
    {
        HitResponseBuilder hb = new HitResponseBuilder().Damage(damage).Target(target);
        if(emp) return hb.Destroy().Build();
        Destroy(gameObject);
        AstarPath.active.Scan();
        return hb.Build();
    }
}
