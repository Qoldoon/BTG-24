using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class SoundTracker
{
    private static GameObject sound;
    private static float time;
    public static void EmitSound(GameObject emmiter)
    {
        sound = emmiter;
        time = Time.time;
    }

    [CanBeNull]
    public static GameObject Listen()
    {
        Cleanup();
        return sound;
    }

    private static void Cleanup()
    {
        if (time + 0.5f < Time.time)
        {
            sound = null;
        }
    }
}
