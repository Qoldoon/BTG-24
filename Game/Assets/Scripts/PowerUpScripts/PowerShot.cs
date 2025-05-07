using UnityEngine;

public class PowerShot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var controller = collision.GetComponent<PlayerController>();
            controller.playerInventory.Amplify();
            controller.playerInventory.canvas.CreateText("Power Shot");
            Destroy(gameObject);
        }
    }
}
