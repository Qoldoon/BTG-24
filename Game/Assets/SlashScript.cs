using System.Collections;
using UnityEngine;

public class SlashScript : MonoBehaviour, IDamagable
{
    public bool isParrying = false;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false)
    {
        return new HitResponseBuilder().ForEnemy().Damage(damage).Reflect().Build();
    }
    public void Slash()
    {

        StartCoroutine(Routine());
    }
    IEnumerator Routine()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        _spriteRenderer.enabled = true;
        _boxCollider2D.enabled = true;
        isParrying = true;
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.enabled = false;
        _boxCollider2D.enabled = false;
        isParrying = false;
    }
}