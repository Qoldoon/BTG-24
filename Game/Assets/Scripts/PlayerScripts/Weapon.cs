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
    private Indicator _reloadIndicator;
    private Coroutine _reloadCoroutine = null;

    private void Start()
    {
        if(PlayerInventory != null)
            _reloadIndicator = PlayerInventory.reloadIndicator;
        _time = Time.time;
        _currentAmmo = ammoCount;
    }
    public void Use()
    {
        if (_currentAmmo == 0) return;
        if (Time.time < _time) return;
        if (_isReloading) return;
        var vector = transform.parent.GetComponent<PlayerController>().lookDirection;
        vector = EnemyBehaviour.RotateVector(vector, Random.Range(-bulletSpread, +bulletSpread));
        SoundTracker.EmitSound(gameObject);
        var bullet = Instantiate(Bullet, transform.position + transform.up * 0.3f, transform.rotation);
        var mult = this.PlayerInventory.multiplier;
        if (bullet.TryGetComponent(out Projectile projectile))
        {
            projectile.direction = vector.normalized;
            projectile.speed = bulletSpeed * mult;
            projectile.damage = bulletDamage * mult;
            projectile.target = 1;
            projectile.emp |= (mult > 1);
        }
        this.PlayerInventory.deAmplify();
        _currentAmmo--;
        _time = Time.time + fireRate;
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
    public void InterruptReload()
    {
        if (_isReloading && _reloadCoroutine != null)
        {
            StopCoroutine(_reloadCoroutine);
            _isReloading = false;
            _reloadIndicator?.Stop();
        }
    }
    private IEnumerator Reload()
    {
        _reloadIndicator?.Fill(reloadTime);
        _isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        _isReloading = false;
        PlayerInventory.reloads -= reloadCost;
        PlayerInventory.canReload = PlayerInventory.reloads > 0;
        _currentAmmo = ammoCount;
    }
}