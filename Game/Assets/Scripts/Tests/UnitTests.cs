using System;
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
        item.AddComponent<SpriteRenderer>();
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
        item.AddComponent<SpriteRenderer>();
        item.AddComponent<Blaster>();

        GameObject item2 = new GameObject("Item");
        item2.AddComponent<SpriteRenderer>();
        item2.AddComponent<Sniper>();

        GameObject item3 = new GameObject("Item");
        item3.AddComponent<SpriteRenderer>();
        item3.AddComponent<Launcher>();

        GameObject item4 = new GameObject("Item");
        item4.AddComponent<SpriteRenderer>();
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

    [Test]
    public void HitResponseBuilder_Defaults()
    {
        var r = new HitResponseBuilder().Build();
        Assert.AreEqual(50, r.damage);
        Assert.AreEqual(0, r.target);
        Assert.IsFalse(r.destroy);
        Assert.IsFalse(r.reflect);
    }

    [Test]
    public void HitResponseBuilder_FluentDamage()
    {
        var r = new HitResponseBuilder().Damage(25).Build();
        Assert.AreEqual(25, r.damage);
    }

    [Test]
    public void HitResponseBuilder_TargetHelpers()
    {
        Assert.AreEqual(0, new HitResponseBuilder().ForPlayer().Build().target);
        Assert.AreEqual(1, new HitResponseBuilder().ForEnemy().Build().target);
        Assert.AreEqual(2, new HitResponseBuilder().ForAll().Build().target);
        Assert.AreEqual(7, new HitResponseBuilder().Target(7).Build().target);
    }

    [Test]
    public void HitResponseBuilder_Flags()
    {
        var r = new HitResponseBuilder().Destroy().Reflect().Build();
        Assert.IsTrue(r.destroy);
        Assert.IsTrue(r.reflect);
    }

    [Test]
    public void HitResponseBuilder_FullChain()
    {
        var r = new HitResponseBuilder().Damage(99).ForEnemy().Destroy().Reflect().Build();
        Assert.AreEqual(99, r.damage);
        Assert.AreEqual(1, r.target);
        Assert.IsTrue(r.destroy);
        Assert.IsTrue(r.reflect);
    }

    [Test]
    public void Inventory_AddFillsSequentially()
    {
        var inv = new Inventory(3);
        var i1 = new GameObject().AddComponent<Blaster>();
        var i2 = new GameObject().AddComponent<Sniper>();

        Assert.AreEqual(0, inv.Add(i1));
        Assert.AreEqual(1, inv.Add(i2));
        Assert.AreEqual(2, inv.Count);
        Assert.IsTrue(inv.Exists(0));
        Assert.IsTrue(inv.Exists(1));
        Assert.IsFalse(inv.Exists(2));
    }

    [Test]
    public void Inventory_RemoveAt_DecrementsCount()
    {
        var inv = new Inventory(3);
        inv.Add(new GameObject().AddComponent<Blaster>());
        inv.Add(new GameObject().AddComponent<Sniper>());

        inv.RemoveAt(0);

        Assert.AreEqual(1, inv.Count);
        Assert.IsFalse(inv.Exists(0));
        Assert.IsTrue(inv.Exists(1));
    }

    [Test]
    public void Inventory_Indexer_ThrowsOutOfRange()
    {
        var inv = new Inventory(2);
        Assert.Throws<IndexOutOfRangeException>(() => { var _ = inv[-1]; });
        Assert.Throws<IndexOutOfRangeException>(() => { var _ = inv[2]; });
    }

    [Test]
    public void Inventory_Enumerator_SkipsEmpty()
    {
        var inv = new Inventory(3);
        inv.Add(new GameObject().AddComponent<Blaster>());
        inv.Add(new GameObject().AddComponent<Sniper>());
        inv.RemoveAt(0);

        int count = 0;
        foreach (var _ in inv) count++;
        Assert.AreEqual(1, count);
    }

    [Test]
    public void Sighting_IsRecent_NullSighting_False()
    {
        Assert.IsFalse(Sighting.IsRecent(null, 0.1f));
    }

    [Test]
    public void Sighting_IsRecent_NullTarget_False()
    {
        var s = new Sighting { Target = null, TimeSeen = Time.time };
        Assert.IsFalse(Sighting.IsRecent(s, 0.1f));
    }

    [Test]
    public void Sighting_IsRecent_Fresh_True()
    {
        var go = new GameObject();
        var s = new Sighting { Target = go, TimeSeen = Time.time };
        Assert.IsTrue(Sighting.IsRecent(s, 0.1f));
    }

    [Test]
    public void Sighting_IsRecent_Old_False()
    {
        var go = new GameObject();
        var s = new Sighting { Target = go, TimeSeen = Time.time - 5f };
        Assert.IsFalse(Sighting.IsRecent(s, 0.1f));
    }

    [Test]
    public void Sound_IsRecent_Null_False()
    {
        Assert.IsFalse(Sound.IsRecent(null, 0.1f));
    }

    [Test]
    public void Sound_IsRecent_Fresh_True()
    {
        var s = new Sound { Position = Vector3.zero, TimeHeard = Time.time };
        Assert.IsTrue(Sound.IsRecent(s, 0.1f));
    }

    [Test]
    public void Sightings_AddPlayer_PopulatesPlayerSighting()
    {
        var sightings = new Sightings();
        var player = new GameObject("Player") { tag = "Player" };
        var s = new Sighting { Target = player, TimeSeen = Time.time };

        sightings.TryAddSighting(s);

        Assert.IsNotNull(sightings.PlayerSighting());
        Assert.AreSame(player, sightings.PlayerSighting().Target);
    }

    [Test]
    public void Sightings_AddSameTargetTwice_Updates()
    {
        var sightings = new Sightings();
        var player = new GameObject("Player") { tag = "Player" };

        sightings.TryAddSighting(new Sighting { Target = player, TimeSeen = 1f, Position = Vector3.zero });
        sightings.TryAddSighting(new Sighting { Target = player, TimeSeen = 2f, Position = Vector3.one });

        Assert.AreEqual(1, sightings.Count);
        Assert.AreEqual(2f, sightings.PlayerSighting().TimeSeen);
        Assert.AreEqual(Vector3.one, sightings.PlayerSighting().Position);
    }

    [Test]
    public void IdleState_NothingToSee_StaysIdle()
    {
        var next = new IdleState().ChangeState(new Sightings());
        Assert.IsInstanceOf<IdleState>(next);
    }

    [Test]
    public void IdleState_SeesPlayer_Attacks()
    {
        var sightings = new Sightings();
        var player = new GameObject("Player") { tag = "Player" };
        var s = new Sighting { Target = player, TimeSeen = Time.time };
        sightings.TryAddSighting(s);

        var next = new IdleState().ChangeState(sightings);

        Assert.IsInstanceOf<AttackState>(next);
    }

    [Test]
    public void IdleState_HearsSound_Investigates()
    {
        var sightings = new Sightings();
        sightings.TryAddSound(new Sound { Position = Vector3.zero, TimeHeard = Time.time });

        var next = new IdleState().ChangeState(sightings);

        Assert.IsInstanceOf<InvestigateState>(next);
    }

    [Test]
    public void AttackState_LosesPlayer_Chases()
    {
        var sightings = new Sightings();
        var player = new GameObject("Player") { tag = "Player" };
        var stale = new Sighting { Target = player, TimeSeen = Time.time - 1f };
        sightings.TryAddSighting(stale);

        var next = new AttackState(sightings).ChangeState(sightings);

        Assert.IsInstanceOf<ChaseState>(next);
    }

    [Test]
    public void EnemyHealth_HitFriendlyFire_Survives()
    {
        var go = new GameObject();
        var enemy = go.AddComponent<EnemyHealth>();
        enemy.health = 50;

        var response = enemy.Hit(50, 0);

        Assert.IsFalse(response.destroy);
        Assert.AreEqual(0, response.target);
    }

    [Test]
    public void EnemyHealth_HitWithShield_NoDamage()
    {
        var go = new GameObject();
        var enemy = go.AddComponent<EnemyHealth>();
        enemy.Shield = true;
        enemy.health = 10;

        enemy.Hit(50, 1);

        Assert.IsTrue(enemy.Shield);
    }

    [Test]
    public void EnemyHealth_EMPRemovesShield()
    {
        var go = new GameObject();
        var enemy = go.AddComponent<EnemyHealth>();
        enemy.Shield = true;

        enemy.Hit(50, 1, emp: true);

        Assert.IsFalse(enemy.Shield);
    }

    [Test]
    public void PlayerInventory_KeyAddCheck()
    {
        var player = new GameObject("Player");
        var inv = player.AddComponent<PlayerInventory>();

        Assert.IsFalse(inv.HasKey("red"));
        inv.AddKey("red");
        Assert.IsTrue(inv.HasKey("red"));
        Assert.IsFalse(inv.HasKey("blue"));
    }

    [Test]
    public void PlayerInventory_Amplify()
    {
        var player = new GameObject("Player");
        var inv = player.AddComponent<PlayerInventory>();

        Assert.AreEqual(1f, inv.multiplier);
        inv.Amplify();
        Assert.AreEqual(2f, inv.multiplier);
        inv.DeAmplify();
        Assert.AreEqual(1f, inv.multiplier);
    }

    [Test]
    public void PlayerInventory_EquipChangesCurrent()
    {
        var player = new GameObject("Player");
        var inv = player.AddComponent<PlayerInventory>();

        var g1 = new GameObject(); g1.AddComponent<SpriteRenderer>(); g1.AddComponent<Blaster>();
        var g2 = new GameObject(); g2.AddComponent<SpriteRenderer>(); g2.AddComponent<Sniper>();
        inv.Add(g1);
        inv.Add(g2);

        inv.Equip(1);

        Assert.AreEqual(1, inv.current);
        inv.IsUsable(out var u);
        Assert.IsTrue(u is Sniper);
    }

    [Test]
    public void PlayerInventory_EquipInvalidSlot_NoOp()
    {
        var player = new GameObject("Player");
        var inv = player.AddComponent<PlayerInventory>();

        inv.Equip(2);

        Assert.AreEqual(0, inv.current);
    }
}
