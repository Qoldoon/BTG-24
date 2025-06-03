using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private Vector2 moveInput;
    private Vector2 smoothedMoveInput;
    private Vector2 moveVelocity;
    private PlayerControls controls;
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;
        controls.Player.Equip.performed += EquipHandler;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
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
        MoveHandler();
        RotationHandler();
        DodgeHandler();
        AttackHandler();
        ParryHandler();
        ReloadHandler();
        InteractHandler();
    }

    private bool IsDead()
    {
        if (dead)
        {
            if (!controls.Player.Reload.IsPressed())
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
        if (!controls.Player.Reload.IsPressed()) return;
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
        if (!controls.Player.AltUse.IsPressed()) return;
        slash.GetComponent<SlashScript>().Slash();
    }
    private void AttackHandler()
    {
        if (!controls.Player.Use.IsPressed()) return;
        if(playerInventory.IsUsable(out IUsable usableItem))
            usableItem.Use();
    }
    private void DodgeHandler()
    {
        if (!controls.Player.Dodge.IsPressed()) return;
        if (Time.time < t) return;
        StartCoroutine(Dodge());
        t = Time.time + 1f;
    }
    private void MoveHandler()
    {
        if (rb == null) return;
        smoothedMoveInput = Vector2.SmoothDamp(
            current: smoothedMoveInput,
            target: moveInput,
            currentVelocity: ref moveVelocity,
            smoothTime: 0.1f
        );
        var moveDir = Vector2.ClampMagnitude(smoothedMoveInput, 1f);
        rb.linearVelocity = moveDir * (speed * speedMult);
    }

    private void EquipHandler(InputAction.CallbackContext context)
    {
        if(playerInventory is null) return;
        var bindingIndex = context.action.GetBindingIndexForControl(context.control) % 3;
        playerInventory.Equip(bindingIndex);
    }

    public event Action Interact;
    private void InteractHandler()
    {
        if (!controls.Player.Interact.IsPressed()) return;
        Interact?.Invoke();
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
