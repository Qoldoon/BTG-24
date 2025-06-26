using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : Item, IUsable
{
    public int ammoCount = 6;
    private int _currentAmmo;
    public float fireRate = 1;
    public float bulletSpeed = 30;
    public int bulletDamage = 50;
    public float bulletSpread;
    public float reloadTime = 1;
    public int reloadCost = 20;
    public GameObject Bullet;
    private bool _isReloading;
    private float _time;
    private Coroutine _reloadCoroutine = null;
    private IActor _wielder;
    private void Start()
    {
        _time = Time.time;
        _currentAmmo = ammoCount;
        _wielder = GetComponentInParent<IActor>();
    }
    public void Use()
    {
        if (_currentAmmo == 0) return;
        if (Time.time < _time) return;
        if (_isReloading) return;
        var vector = _wielder.GetLookDirection();
        vector = transform.up;
        vector = EnemyBehaviour.RotateVector(vector, Random.Range(-bulletSpread, +bulletSpread));
        SoundTracker.TriggerGunShot(transform.position);
        var bullet = Instantiate(Bullet, transform.position + transform.up * 0.3f, transform.rotation);
        var mult = Multiplier();
        if (bullet.TryGetComponent(out Projectile projectile))
        {
            projectile.direction = vector.normalized;
            projectile.speed = bulletSpeed * mult;
            projectile.damage = bulletDamage * mult;
            projectile.target = _wielder.Target();
            projectile.emp |= (mult > 1);
        }
        if(PlayerInventory is not null)
            PlayerInventory.DeAmplify();
        _currentAmmo--;
        _time = Time.time + fireRate;
    }

    private float Multiplier()
    {
        if (PlayerInventory is null) return 1;
        return PlayerInventory.multiplier;
    }

    public void SecondaryUse()
    {
        if(_isReloading) return;
        _reloadCoroutine = StartCoroutine(Reload());
    }

    public bool NeedsReload()
    {
        return _currentAmmo < ammoCount;
    }
    public override void Equip()
    {
        base.Equip();
        _time = Time.time + 0.5f;
        var canvas = PlayerInventory.canvas;
        canvas?.CreateText(name);
    }

    public override void UnEquip()
    {
        InterruptReload();
    }

    public override void OnAdd(PlayerInventory inventory, int index)
    {
        base.OnAdd(inventory, index);
        _wielder = inventory.transform.GetComponent<IActor>();
    }

    public void InterruptReload()
    {
        if (_isReloading && _reloadCoroutine != null)
        {
            StopCoroutine(_reloadCoroutine);
            _isReloading = false;
            PlayerInventory.reloadIndicator?.Stop();
        }
    }
    private IEnumerator Reload()
    {
        PlayerInventory.reloadIndicator?.Fill(reloadTime);
        _isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        _isReloading = false;
        PlayerInventory.Reload(reloadCost);
        _currentAmmo = ammoCount;
    }
}