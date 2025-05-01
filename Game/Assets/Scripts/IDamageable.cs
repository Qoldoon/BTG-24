public interface IDamageable
{
    //0 - player, 1 - enemy
    HitResponse Hit(float damage, int target, bool emp = false);
}
