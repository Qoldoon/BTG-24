using UnityEngine;

public interface IDamagable
{
    //0 - player, 1 - enemy
    bool Hit(float damage, int target, bool emp = false);
}
