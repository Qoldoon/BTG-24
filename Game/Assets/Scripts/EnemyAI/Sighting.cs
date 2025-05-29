using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public override string ToString()
    {
        return $"{Target.ToString()},  seen: {TimeSeen}";
    }
}

public class Sound
{
    public Vector3 Position;
    public float TimeHeard;

    public bool Similar(Sound sound)
    {
        return Vector3.Distance(this.Position, sound.Position) < 1f;
    }
    public static bool IsRecent(Sound sound, float threshold)
    {
        if (sound == null) return false;
        return Mathf.Abs(sound.TimeHeard - Time.time) <= threshold;
    }
    public override string ToString()
    {
        return $"{Position.ToString()},  heard: {TimeHeard}";
    }
}

public class Sightings : IEnumerable<Sighting>
{
    private List<Sighting> _sightings;
    private List<Sound> _sounds;
    private Sighting _playerSighting;
    private Sighting _allySighting;
    public int Count => _sightings.Count;
    public Sightings()
    {
        _sightings = new List<Sighting>();
        _sounds = new List<Sound>();
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

    public List<Sighting> WallSearch()
    {
        Forget();
        return _sightings.FindAll(s => s.Target.CompareTag("Walls"));
    }

    public Sound Listen()
    {
        _sounds.RemoveAll(s => s.TimeHeard + 2 < Time.time);
        return _sounds.OrderByDescending(s => s.TimeHeard).FirstOrDefault();;
    }
    private void Forget()
    {
        _sightings.RemoveAll(s => s.Target == null);
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

    public void TryAddSound(Sound sound)
    {
        var existingSound = _sounds.Find(s => s.Similar(sound));
        if(existingSound == null)
            _sounds.Add(sound);
        else
        {
            existingSound.Position = sound.Position;
            existingSound.TimeHeard = sound.TimeHeard;
        }
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
