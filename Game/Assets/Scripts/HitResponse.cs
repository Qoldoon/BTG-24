using UnityEngine;

public class HitResponse
{
    public float damage;
    public int target;
    public bool destroy;
    public bool reflect;

    public HitResponse(float damage, int target, bool destroy, bool reflect)
    {
        this.damage = damage;
        this.target = target;
        this.destroy = destroy;
        this.reflect = reflect;
    }
}

public class HitResponseBuilder
{
    private float damage;
    private int target;
    private bool destroy;
    private bool reflect;
    
    public HitResponseBuilder()
    {
        damage = 50;
        target = 0;
        destroy = false;
        reflect = false;
    }

    public HitResponseBuilder Damage(float damage)
    {
        this.damage = damage;
        return this;
    }

    public HitResponseBuilder Target(int target)
    {
        this.target = target;
        return this;
    }

    public HitResponseBuilder ForEnemy()
    {
        target = 1;
        return this;
    }
    
    public HitResponseBuilder ForPlayer()
    {
        target = 0;
        return this;
    }
    
    public HitResponseBuilder ForAll()
    {
        target = 2;
        return this;
    }

    public HitResponseBuilder Destroy()
    {
        destroy = true;
        return this;
    }

    public HitResponseBuilder Reflect()
    {
        reflect = true;
        return this;
    }
    
    public HitResponse Build()
    {
        return new HitResponse(damage, target, destroy, reflect);
    }
}