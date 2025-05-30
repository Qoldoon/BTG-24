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
    public Vector2 lookDirection { get; private set; }
    public PlayerInventory playerInventory;
    public GameObject slash;
    bool dead = false;
    public bool freeze;
    
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }
    void Update()
    {
        if(freeze)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = 0;
            return;
        }
        if (IsDead()) return;
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerInventory.Toss(lookDirection * 15);
        }
        ParryHandler();
        AttackHandler();
        DodgeHandler();
        MoveHandler();
        RotationHandler();
        ReloadHandler();
        EquipHandler();
    }

    private bool IsDead()
    {
        if (dead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            return true;
        }

        return false;
    }

    private void ReloadHandler()
    {
        if (!Input.GetKey(KeyCode.R)) return;
        if (!playerInventory.canReload) return;
        if(playerInventory.IsUsable(out IUsable usableItem))
            usableItem.SecondaryUse();
    }
    private void RotationHandler()
    {
        if (rb is null) return;
        mouse_pos = Input.mousePosition;
        object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x -= object_pos.x;
        mouse_pos.y -= object_pos.y;
        lookDirection = new  Vector2(mouse_pos.x, mouse_pos.y).normalized;
        var angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }
    private void ParryHandler()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        slash.GetComponent<SlashScript>().Slash();
    }
    private void AttackHandler()
    {
        if (!Input.GetKey(KeyCode.Mouse0)) return;
        if(playerInventory.IsUsable(out IUsable usableItem))
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
        if (rb is null) return;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 moveDir = Vector2.ClampMagnitude(new Vector2(x, y), 1);
        rb.linearVelocity = moveDir * (speed * speedMult);
    }

    private void EquipHandler()
    {
        if(playerInventory is null) return;
        if (Input.GetKeyDown(KeyCode.Alpha1)) { playerInventory.Equip(0); return; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { playerInventory.Equip(1); return; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { playerInventory.Equip(2); return; }
    }
    IEnumerator Dodge()
    {
        var orgSpeed = speed;
        speed = 25f;
        yield return new WaitForSeconds(0.15f/speedMult);
        speed = orgSpeed;
    }
    public HitResponse Hit(float damage, int target, bool emp = false)
    {
        HitResponseBuilder hb = new HitResponseBuilder().Damage(damage).Target(target);
        if (target == 1) return hb.Build();
        hitPoints--;
        
        if (hitPoints < 1)
        {
            Die();
        }
        return hb.Destroy().Build();
    }
    public void Die()
    {
        playerInventory?.playerUI.TitleText("DEAD");
        Time.timeScale = 0;
        dead = true;
    }
   
}
