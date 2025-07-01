using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected PlayerController _player;
    protected bool _ready;

    protected virtual void Interact()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        _ready = true;
        _player = other.gameObject.GetComponent<PlayerController>();
        _player.Interact += Interact;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        _ready = false;
        _player.Interact -= Interact;
        _player = null;
    }
    
    
}