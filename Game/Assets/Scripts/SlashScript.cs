using System;
using System.Collections;
using UnityEngine;

public class SlashScript : MonoBehaviour, IDamageable
{
    private SpriteRenderer _spriteRenderer;

    private float t;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Resources.Load<Sprite>("Textures/slash");
    }

    private void Start()
    {
        t = Time.time + 0.2f;
    }

    private void Update()
    {
        if(t < Time.time)
            Destroy(gameObject);
    }

    public HitResponse Hit(float damage, int target, bool emp = false)
    {
        var hb = new HitResponseBuilder().Target(target).Damage(damage);
        if (target == 1) return hb.Build();
        t = Mathf.Min(t + 0.1f, Time.time + 0.4f);
        return hb.ForEnemy().Reflect().Build();
    }
}