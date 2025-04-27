using System;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    void Awake()
    {
        objectToSpawn = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        // try
        // {
        //     Camera.main.GetComponent<Camera_Follow>().target = objectToSpawn.transform;
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        // }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
