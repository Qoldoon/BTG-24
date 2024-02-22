using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector3 mouse_pos;
    public Transform target;
    Vector3 object_pos;
    float angle;
 
    // Update is called once per frame
    void Update()
    {
        mouse_pos = Input.mousePosition;
        object_pos = Camera.main.WorldToScreenPoint(target.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }
}
