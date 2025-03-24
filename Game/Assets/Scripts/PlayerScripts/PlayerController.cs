using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamagable
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
    public GameObject slash;
    public bool IsParrying()
    {
        return isParrying;
    }

    private void Start()
    {

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
        // Add parry animation or effects here
        StartCoroutine(EndParry());
    }
    private void AttackHandler()
    {
        if (!Input.GetKey(KeyCode.Mouse0)) return;
        // ifGun
        GetComponent<PlayerInventory>().GetCurrent().Attack();
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
        rb.linearVelocity = moveDir * (speed * speedMult);
    }
    IEnumerator Dodge()
    {
        var orgSpeed = speed;
        speed = 25f;
        yield return new WaitForSeconds(0.15f/speedMult);
        speed = orgSpeed;
    }
    IEnumerator EndParry()
    {
        isParrying = true;
        yield return new WaitForSeconds(parryDuration);
        isParrying = false;
    }

    public HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false)
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
