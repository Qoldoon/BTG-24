using System;
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
}
