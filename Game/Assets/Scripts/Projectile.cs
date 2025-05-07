using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [NonSerialized]
    public Vector2 direction;
    [NonSerialized]
    public float speed;
    [NonSerialized]
    public float damage;
    [NonSerialized]
    public int target;
    [NonSerialized] 
    public bool emp;
}