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
    
    [Test]
    public void Inventory_AddItem()
    {
        GameObject player = new GameObject("Player");
        var inv = player.AddComponent<PlayerInventory>();
        
        Assert.That(inv, Is.Not.Null);
        Assert.That(inv.slots, Is.Empty);
        
        GameObject item = new GameObject("Item");
        item.AddComponent<Blaster>();
        
        inv.Add(item);
        
        Assert.That(inv.slots, Is.Not.Empty);
        Assert.IsTrue(player.transform.childCount == 1);
    }

    [Test]
    public void Inventory_Overfill()
    {
        GameObject player = new GameObject("Player");
        var inv = player.AddComponent<PlayerInventory>();
        
        GameObject item = new GameObject("Item");
        item.AddComponent<Blaster>();
        
        GameObject item2 = new GameObject("Item");
        item2.AddComponent<Sniper>();
        
        GameObject item3 = new GameObject("Item");
        item3.AddComponent<Launcher>();
        
        GameObject item4 = new GameObject("Item");
        item4.AddComponent<Grenade>();
        
        inv.Add(item);
        inv.Add(item2);
        inv.Add(item3);
        
        Assert.That(inv.slots, Is.Not.Empty);
        Assert.IsTrue(player.transform.childCount == 3);
        
        inv.Add(item4);
        
        inv.IsUsable(out var usable);
        Assert.IsTrue(usable is Grenade);
    }
}
