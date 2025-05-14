using System.Collections;
using EnemyAI;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTests
{
    
    [Test]
    public void PlayerHit_LosesHP()
    {
        GameObject player = new GameObject();
        player.AddComponent<PlayerController>();
        var playerController= player.GetComponent<PlayerController>();
        
        Assert.AreEqual(playerController.hitPoints, 1);
        playerController.Hit(50, 1);
        Assert.AreEqual(playerController.hitPoints, 1);
        playerController.Hit(50, 0);
        Assert.AreEqual(playerController.hitPoints, 0);
        Time.timeScale = 1;
    }
    
    [UnityTest]
    public IEnumerator EnemyHit_Dies()
    {
        var go = new GameObject();
        var enemy = go.AddComponent<EnemyHealth>();

        enemy.Hit(50, 1);
        
        yield return null;

        Assert.IsTrue(go == null);
    }
}
