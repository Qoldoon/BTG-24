using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BigWall : MonoBehaviour
{
    private bool scan;
    private float time;
    public void Update()
    {
        if(scan && Time.time >= time)
        {
            AstarPath.active?.Scan();
            scan = false;
        }
    }

    public void Scan()
    {
        if(!scan)
            time = Time.time + 0.2f;
        scan = true;
    }
}
