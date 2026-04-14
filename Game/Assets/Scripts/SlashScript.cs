using System;
using System.Collections;
using UnityEngine;

public class SlashScript : MonoBehaviour, IDamageable
{
    private SpriteRenderer _spriteRenderer;
    private float t;
    private float _startTime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Resources.Load<Sprite>("Textures/slash");
    }

    private void Start()
    {
        _startTime = Time.time;
        t = Time.time + 0.2f;
    }

    private void Update()
    {
        if (t < Time.time)
        {
            Destroy(gameObject);
            return;
        }

        float age = Time.time - _startTime;
        float scale;
        if (age < 0.04f)
            scale = Mathf.Lerp(0f, 1.15f, age / 0.04f);
        else if (age < 0.07f)
            scale = Mathf.Lerp(1.15f, 1f, (age - 0.04f) / 0.03f);
        else
            scale = 1f;
        transform.localScale = Vector3.one * scale;

        var c = _spriteRenderer.color;
        c.a = Mathf.Clamp01((t - Time.time) / 0.1f);
        _spriteRenderer.color = c;
    }

    public HitResponse Hit(float damage, int target, bool emp = false)
    {
        var hb = new HitResponseBuilder().Target(target).Damage(damage);
        if (target == 1) return hb.Build();
        t = Mathf.Min(t + 0.1f, Time.time + 0.4f);
        return hb.ForEnemy().Reflect().Build();
    }
}