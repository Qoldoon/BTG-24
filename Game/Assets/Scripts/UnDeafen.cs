using UnityEngine;

public class UnDeafen : MonoBehaviour
{
    EnemyBehaviour[] enemies;
    void Start()
    {
        enemies = GetComponentsInChildren<EnemyBehaviour>();
    }
    
    public void Trigger()
    {
        foreach (var b in enemies)
        {
            b.isDeaf = false;
        }
    }
}
