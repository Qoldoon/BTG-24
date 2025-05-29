using UnityEngine;

public class TossPhysics : MonoBehaviour
{
    public Vector2 direction;
    private float time;
    Rigidbody2D rb;
    BoxCollider2D coll;
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.linearDamping = 5;
        rb.angularDamping = 3;
        rb.gravityScale = 0;
        rb.linearVelocity = direction;
        rb.angularVelocity = 300;
        
        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.size = new  Vector2(0.4f, 0.4f);
        coll.isTrigger = false;
        coll.excludeLayers = 1 << LayerMask.NameToLayer("Default");
        
        time = Time.time + 5f;
    }
    
    void Update()
    {
        if(time <= Time.time || rb.linearVelocity.magnitude < 0.1f)
        {
            Destroy(rb);
            Destroy(coll);
            Destroy(this);
        }
    }
}
