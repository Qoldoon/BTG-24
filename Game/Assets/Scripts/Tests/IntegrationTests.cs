using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class IntegrationTests
{
    [TearDown]
    public void TearDown()
    {
        foreach (var eh in Object.FindObjectsOfType<EnemyHealth>())
            Object.DestroyImmediate(eh.gameObject);
        foreach (var bs in Object.FindObjectsOfType<BulletScript>())
            Object.DestroyImmediate(bs.gameObject);
    }

    [UnityTest]
    public IEnumerator BulletHitsEnemy_EnemyDies()
    {
        Time.timeScale = 1;
        GameObject enemy = new GameObject("Enemy");
        enemy.AddComponent<Rigidbody2D>().gravityScale = 0;
        enemy.AddComponent<EnemyHealth>();
        enemy.AddComponent<BoxCollider2D>();
        enemy.tag = "Enemy";

        GameObject projectile = new GameObject("Projectile");
        projectile.layer = LayerMask.NameToLayer("Ignore Raycast");
        projectile.AddComponent<Rigidbody2D>().gravityScale = 0;
        projectile.AddComponent<SpriteRenderer>();
        projectile.AddComponent<BoxCollider2D>();
        var b = projectile.AddComponent<BulletScript>();
        b.target = 1;
        b.speed = 5;
        b.damage = 50;
        b.direction = new Vector3(1, 0, 0);

        enemy.transform.position = Vector3.zero;
        projectile.transform.position = new Vector3(-1, 0, 0);

        yield return new WaitForSeconds(0.1f);

        Assert.IsFalse(projectile.transform.position == new Vector3(-1, 0, 0));
        Assert.IsTrue(enemy == null);
    }

    [UnityTest]
    public IEnumerator BulletHitsWall_Stops()
    {
        Time.timeScale = 1;
        var wall = new GameObject("Wall");
        wall.AddComponent<BoxCollider2D>();
        wall.transform.position = Vector3.zero;

        var projectile = new GameObject("Projectile");
        projectile.layer = LayerMask.NameToLayer("Ignore Raycast");
        projectile.AddComponent<SpriteRenderer>();
        projectile.AddComponent<BoxCollider2D>();
        var b = projectile.AddComponent<BulletScript>();
        b.target = 1;
        b.speed = 5;
        b.damage = 50;
        b.direction = new Vector3(1, 0, 0);
        projectile.transform.position = new Vector3(-1, 0, 0);

        yield return new WaitForSeconds(0.5f);

        Assert.IsTrue(projectile == null);
    }

    [UnityTest]
    public IEnumerator BulletTargetsPlayer_EnemySurvives()
    {
        Time.timeScale = 1;
        var enemy = new GameObject("Enemy");
        enemy.AddComponent<Rigidbody2D>().gravityScale = 0;
        enemy.AddComponent<EnemyHealth>();
        enemy.AddComponent<BoxCollider2D>();
        enemy.tag = "Enemy";
        enemy.transform.position = Vector3.zero;

        var projectile = new GameObject("Projectile");
        projectile.layer = LayerMask.NameToLayer("Ignore Raycast");
        projectile.AddComponent<Rigidbody2D>().gravityScale = 0;
        projectile.AddComponent<SpriteRenderer>();
        projectile.AddComponent<BoxCollider2D>();
        var b = projectile.AddComponent<BulletScript>();
        b.target = 0;
        b.speed = 5;
        b.damage = 50;
        b.direction = new Vector3(1, 0, 0);
        projectile.transform.position = new Vector3(-1, 0, 0);

        yield return new WaitForSeconds(0.3f);

        Assert.IsTrue(enemy != null);
    }

    [UnityTest]
    public IEnumerator ShieldedEnemyBullet_SurvivesOneHit()
    {
        Time.timeScale = 1;
        var enemy = new GameObject("Enemy");
        enemy.AddComponent<Rigidbody2D>().gravityScale = 0;
        var health = enemy.AddComponent<EnemyHealth>();
        enemy.AddComponent<BoxCollider2D>();
        enemy.tag = "Enemy";
        health.Shield = true;
        health.health = 50;
        enemy.transform.position = Vector3.zero;

        var projectile = new GameObject("Projectile");
        projectile.layer = LayerMask.NameToLayer("Ignore Raycast");
        projectile.AddComponent<Rigidbody2D>().gravityScale = 0;
        projectile.AddComponent<SpriteRenderer>();
        projectile.AddComponent<BoxCollider2D>();
        var b = projectile.AddComponent<BulletScript>();
        b.target = 1;
        b.speed = 5;
        b.damage = 50;
        b.direction = new Vector3(1, 0, 0);
        projectile.transform.position = new Vector3(-1, 0, 0);

        yield return new WaitForSeconds(0.3f);

        Assert.IsTrue(enemy != null);
        Assert.IsTrue(health.Shield);
    }

    [UnityTest]
    public IEnumerator EMPBullet_StripsShield()
    {
        Time.timeScale = 1;
        var enemy = new GameObject("Enemy");
        enemy.AddComponent<Rigidbody2D>().gravityScale = 0;
        var health = enemy.AddComponent<EnemyHealth>();
        enemy.AddComponent<BoxCollider2D>();
        enemy.tag = "Enemy";
        health.Shield = true;
        enemy.transform.position = Vector3.zero;

        var projectile = new GameObject("Projectile");
        projectile.layer = LayerMask.NameToLayer("Ignore Raycast");
        projectile.AddComponent<Rigidbody2D>().gravityScale = 0;
        projectile.AddComponent<SpriteRenderer>();
        projectile.AddComponent<BoxCollider2D>();
        var b = projectile.AddComponent<BulletScript>();
        b.target = 1;
        b.speed = 20;
        b.damage = 50;
        b.emp = true;
        b.direction = new Vector3(1, 0, 0);
        projectile.transform.position = new Vector3(-1, 0, 0);

        yield return new WaitForSeconds(0.3f);

        Assert.IsFalse(health.Shield);
    }

    [UnityTest]
    public IEnumerator TwoEnemies_BulletKillsFirstOnly()
    {
        Time.timeScale = 1;
        var e1 = new GameObject("Enemy1");
        e1.AddComponent<Rigidbody2D>().gravityScale = 0;
        e1.AddComponent<EnemyHealth>();
        e1.AddComponent<BoxCollider2D>();
        e1.tag = "Enemy";
        e1.transform.position = Vector3.zero;

        var e2 = new GameObject("Enemy2");
        e2.AddComponent<Rigidbody2D>().gravityScale = 0;
        e2.AddComponent<EnemyHealth>();
        e2.AddComponent<BoxCollider2D>();
        e2.tag = "Enemy";
        e2.transform.position = new Vector3(3, 0, 0);

        var projectile = new GameObject("Projectile");
        projectile.layer = LayerMask.NameToLayer("Ignore Raycast");
        projectile.AddComponent<Rigidbody2D>().gravityScale = 0;
        projectile.AddComponent<SpriteRenderer>();
        projectile.AddComponent<BoxCollider2D>();
        var b = projectile.AddComponent<BulletScript>();
        b.target = 1;
        b.speed = 20;
        b.damage = 50;
        b.direction = new Vector3(1, 0, 0);
        projectile.transform.position = new Vector3(-1, 0, 0);

        yield return new WaitForSeconds(0.5f);

        Assert.IsTrue(e1 == null);
        Assert.IsTrue(e2 != null);
    }
}
