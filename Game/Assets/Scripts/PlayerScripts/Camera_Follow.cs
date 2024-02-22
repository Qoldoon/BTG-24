using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target;
    public float smooth = 0.15f;
    Vector3 velocity = new Vector3(0, 0, 0);
    Vector3 mouse_pos;
    Vector3 object_pos;
    void FixedUpdate()
    {
        mouse_pos = Input.mousePosition;
        object_pos = Camera.main.WorldToScreenPoint(target.position);

        if (mouse_pos.x < 0) mouse_pos.x = 0;
        if (mouse_pos.x > Camera.main.pixelWidth) mouse_pos.x = Camera.main.pixelWidth;
        if (mouse_pos.y < 0) mouse_pos.y = 0;
        if (mouse_pos.y > Camera.main.pixelHeight) mouse_pos.y = Camera.main.pixelHeight;

        Vector3 position = new Vector3((mouse_pos.x + object_pos.x) / 2, (mouse_pos.y + object_pos.y) / 2);
        
        
        position = Camera.main.ScreenToWorldPoint(position);
        position.z = -10;

        transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, smooth);
    }
}
