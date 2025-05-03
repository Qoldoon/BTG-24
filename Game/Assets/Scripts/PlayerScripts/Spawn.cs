using System;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    void Awake()
    {
        var str = objectToSpawn.name;
        objectToSpawn = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        objectToSpawn.name = str;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
