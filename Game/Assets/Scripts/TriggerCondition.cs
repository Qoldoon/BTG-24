using UnityEngine;

public class TriggerCondition : MonoBehaviour
{
    public string key = "exit";
    public bool Condition(GameObject target)
    {
        return target.GetComponent<PlayerInventory>().HasKey(key);
    }
}
