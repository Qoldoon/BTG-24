using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Sighting
{
    public GameObject Target;
    public Vector3 Velocity;
    public Vector3 Position;
    public float TimeSeen;


    public override bool Equals(object obj)
    {
        return obj is Sighting sighting && Target.Equals(sighting.Target);
    }

    public override int GetHashCode()
    {
        return Target.GetHashCode();
    }
    
    public static bool IsRecent(Sighting sighting, float threshold)
    {
        if (sighting == null || sighting.Target == null) return false;
        return Mathf.Abs(sighting.TimeSeen - Time.time) <= threshold;
    }
}

public class Sightings : IEnumerable<Sighting>
{
    private List<Sighting> _sightings;
    private Sighting _playerSighting;
    private Sighting _allySighting;
    public int Count => _sightings.Count;
    public Sightings()
    {
        _sightings = new List<Sighting>();
    }

    public Sighting PlayerSighting()
    {
        // Forget();
        return _playerSighting;
    }

    public Sighting AllySighting()
    {
        // Forget();
        return _allySighting;
    }

    public Sighting Sound()
    {
        foreach (var sighting in _sightings)
        {
            if(sighting.Target == null)
                continue;
            if(sighting.Target.CompareTag("Weapon"))
                return sighting;
        }
        return null;
    }
    private void Forget()
    {
        _sightings.RemoveAll(s => Time.time - s.TimeSeen > 12);
        if(_playerSighting.TimeSeen > 12)
            _playerSighting = null;
        if(_allySighting.TimeSeen > 12)
            _allySighting = null;
    }
    
    public void TryAddSighting(Sighting sighting)
    {
        var existingSighting = _sightings.Find(s => s.Equals(sighting));
        if (existingSighting == null)
            _sightings.Add(sighting);
        else
        {
            existingSighting.TimeSeen = sighting.TimeSeen;
            existingSighting.Velocity = sighting.Velocity;
            existingSighting.Position = sighting.Position;
        }
        if(sighting.Target.CompareTag("Player"))
            _playerSighting = existingSighting;
        if(sighting.Target.CompareTag("Enemy"))
            _allySighting = existingSighting;
    }

    public IEnumerator<Sighting> GetEnumerator()
    {
        return _sightings.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
