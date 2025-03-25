using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float damage;
    public int target;
}