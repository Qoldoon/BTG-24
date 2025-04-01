using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] public int hitPoints = 1;
    [SerializeField] public float speedMult = 1f;
    [SerializeField] public float speed = 5f;
    public Rigidbody2D rb;
    private float t;
    Vector3 mouse_pos;
    Vector3 object_pos;
    float angle;
    private PlayerInventory playerInventory;
    public GameObject slash;

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }
    void Update()
    {
        Parry();
        AttackHandler();
        DodgeHandler();
        MoveHandler();
        Look();
        Reload();
    }

    private void Reload()
    {
        if (!Input.GetKey(KeyCode.R)) return;
        if (!playerInventory.canReload) return;
        if(playerInventory.isUsable(out IUsable usableItem))
            usableItem.SecondaryUse();
    }
    private void Look()
    {
        mouse_pos = Input.mousePosition;
        object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x -= object_pos.x;
        mouse_pos.y -= object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }
    private void Parry()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        slash.GetComponent<SlashScript>().Slash();
    }
    private void AttackHandler()
    {
        if (!Input.GetKey(KeyCode.Mouse0)) return;
        if(playerInventory.isUsable(out IUsable usableItem))
            usableItem.Use();
    }
    private void DodgeHandler()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        if (Time.time < t) return;
        StartCoroutine(Dodge());
        t = Time.time + 1f;
    }
    private void MoveHandler()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 moveDir = Vector2.ClampMagnitude(new Vector2(x, y), 1);
        rb.linearVelocity = moveDir * (speed * speedMult);
    }
    IEnumerator Dodge()
    {
        var orgSpeed = speed;
        speed = 25f;
        yield return new WaitForSeconds(0.15f/speedMult);
        speed = orgSpeed;
    }
    public HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false, float radius = 0)
    {
        HitResponseBuilder hb = new HitResponseBuilder().Damage(damage).Target(target);
        if (target == 1) return hb.Build();
        // hitPoints--;
        
        if (hitPoints < 1)
        {
            Die(0);
        }
        return hb.Destroy().Build();
    }
    public void Die(int scene)
    {
        Destroy(GameObject.Find("SelectedItems"));
        speed = 5f;
        SceneManager.LoadScene(scene);
    }
   
}
