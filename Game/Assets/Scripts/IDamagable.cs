using UnityEngine;

public interface IDamagable
{
    //0 - player, 1 - enemy
    HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false);
}
