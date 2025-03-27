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
    public GameObject Bullet;
    private bool _isReloading;
    float time;
    private Indicator reloadIndicator;
    private Coroutine _reloadCoroutine = null;

    private void Start()
    {
        reloadIndicator = transform.parent.gameObject.GetComponentInChildren<Indicator>();
        time = Time.time;
        _currentAmmo = ammoCount;
    }
    public void Use()
    {
        if (_currentAmmo == 0) return;
        if (Time.time < time) return;
        if (_isReloading) return;

        Vector3 euler = transform.eulerAngles;
        var o = euler;
        euler.z = Random.Range(euler.z - bulletSpread, euler.z + bulletSpread);
        transform.eulerAngles = euler;

        var bullet = Instantiate(Bullet, transform.position + transform.up * 0.3f, Quaternion.identity);
        if (bullet.TryGetComponent(out Projectile projectile))
        {
            projectile.direction = (transform.rotation * Vector2.up).normalized;
            projectile.speed = bulletSpeed;
            projectile.damage = bulletDamage;
            projectile.target = 1;
        }
        
        _currentAmmo--;
        time = Time.time + fireRate;
        transform.eulerAngles = o;
    }

    public void SecondaryUse()
    {
        if(_isReloading) return;
        _reloadCoroutine = StartCoroutine(Reload());
    }
    public void InterruptReload()
    {
        if (_isReloading && _reloadCoroutine != null)
        {
            StopCoroutine(_reloadCoroutine);
            _isReloading = false;
            reloadIndicator.Stop();
        }
    }
    private IEnumerator Reload()
    {
        reloadIndicator.Fill(reloadTime);
        _isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        _isReloading = false;
        _currentAmmo = ammoCount;
    }
}