using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class SoundTracker
{
    public delegate void GunShotHandler(Vector3 position);

    public static event GunShotHandler OnGunShot;
    
    public static void TriggerGunShot(Vector3 shotPosition)
    {
        OnGunShot?.Invoke(shotPosition);
    }
}
