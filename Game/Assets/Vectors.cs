using UnityEngine;

public class Vectors : MonoBehaviour
{

    public Transform first;
    public Transform second;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            second.position = transform.position + Behaviour.RightVector(second.position - transform.position);
        }
    }
}
