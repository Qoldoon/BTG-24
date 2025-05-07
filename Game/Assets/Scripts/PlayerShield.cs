using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    PlayerController player;
    SpriteRenderer sprite;

    void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        sprite.enabled = player.hitPoints > 1;
    }
}
