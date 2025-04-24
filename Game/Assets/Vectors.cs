using UnityEngine;

public class Vectors : MonoBehaviour
{

    public Transform first;
    public Transform second;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private float currentAngle = 0f;
    // Update is called once per frame
    void Update()
    {
        float angularSpeed = 360/2f;
        
        currentAngle = angularSpeed * Time.deltaTime;

        first.position = transform.position + Behaviour.RotateVector(first.position - transform.position, currentAngle);
        second.position = transform.position + Behaviour.RotateVector(second.position - transform.position, -currentAngle*2);
    }
}
