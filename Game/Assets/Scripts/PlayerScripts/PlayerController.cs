using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public int hitPoints = 1;
    [SerializeField] public float speedMult = 1f;
    [SerializeField] public float speed = 5f;
    public Rigidbody2D rb;
    private float t;
    private bool isParrying = false;
    public float parryDuration = 0.2f;
    Vector3 mouse_pos;
    Vector3 object_pos;
    float angle;
    public bool IsParrying()
    {
        return isParrying;
    }
    void Update()
    {
        Parry();
        AttackHandler();
        dodgeHandler();
        moveHandler();
        Look();
    }
    private void Look()
    {
        mouse_pos = Input.mousePosition;
        object_pos = Camera.main.WorldToScreenPoint(GetComponent<Transform>().position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }
    private void Parry()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        isParrying = true;
        // Add parry animation or effects here
        Invoke("EndParry", parryDuration);
    }
    private void AttackHandler()
    {
        if (!Input.GetKey(KeyCode.Mouse0)) return;
        var weapon = PlayerInventory.items[GetComponentInChildren<PlayerInventory>().current];
        if (!weapon.isGun && !weapon.isGrenade) return;
        if (weapon.isGun) weapon.go.GetComponent<Shoot>().Attack();
        if (weapon.isGrenade) weapon.go.GetComponent<Throw>().Attack();
    }
    private void dodgeHandler()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        if (Time.time < t) return;
        StartCoroutine(Dodge());
        t = Time.time + 1f;
    }
    private void moveHandler()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 moveDir = Vector2.ClampMagnitude(new Vector2(x, y), 1);
        rb.linearVelocity = moveDir * speed * speedMult;
    }
    IEnumerator Dodge()
    {
        var orgSpeed = speed;
        speed = 25f;
        yield return new WaitForSeconds(0.15f/speedMult);
        speed = orgSpeed;
    }
    private void EndParry()
    {
        isParrying = false;
    }
    public void Die(int scene)
    {
        Destroy(GameObject.Find("SelectedItems"));
        speed = 5f;
        SceneManager.LoadScene(scene);
    }
   
}
