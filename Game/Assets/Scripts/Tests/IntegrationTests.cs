using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class IntegrationTests
{
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
}
