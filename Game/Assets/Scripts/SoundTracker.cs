using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class SoundTracker
{
    private static GameObject sound;
    public static void EmitSound(GameObject emmiter)
    {
        sound = emmiter;
    }

    [CanBeNull]
    public static GameObject Listen()
    {
        return sound;
    }
}
