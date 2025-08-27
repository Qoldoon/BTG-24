using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IActor, IDamageable
{
    [SerializeField] public int hitPoints = 1;
    [SerializeField] public float speedMult = 1f;
    private float currentSpeedMult = 1f;
    private float speedMultVelocity = 0f;
    [SerializeField] public float speed = 5f;
    public Rigidbody2D rb;
    private EnumArray<Timer, float> t = new ();
    Vector3 mouse_pos;
    Vector3 object_pos;
    public Vector2 lookDirection { get; private set; }
    public PlayerInventory playerInventory;

    bool dead = false;
    public bool freeze;
    private Vector2 moveInput;
    private Vector2 smoothedMoveInput;
    private Vector2 moveVelocity;
    private PlayerControls controls;
    public Camera Camera;

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
        if (Freeze()) return;
        TossHandler();
        MoveHandler();
        RotationHandler();
        DodgeHandler();
        AttackHandler();
        ParryHandler();
        ReloadHandler();
        InteractHandler();
    }

    private void TossHandler()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerInventory.Toss(lookDirection * 15);
        }
    }

    private bool Freeze()
    {
        if(freeze)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = 0;
            return true;
        }

        if (IsDead()) return true;
        return false;
    }

    private bool IsDead()
    {
        if (dead)
        {
            if (controls.Player.Reload.IsPressed())
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
        if (!playerInventory.NeedsReload()) return;
        if (playerInventory.IsUsable(out IUsable usableItem))
            usableItem.SecondaryUse();
    }
    private void RotationHandler()
    {
        if (rb is null) return;
        Vector2 mousePos = controls.Player.Mouse.ReadValue<Vector2>();
        Vector3 objectPos = Camera.WorldToScreenPoint(transform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;
        lookDirection = mousePos.normalized;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }
    private void ParryHandler()
    {
        if (!controls.Player.AltUse.WasPressedThisFrame()) return;
        if (Time.time < t[Timer.Parry]) return;
        if (GetComponentInChildren<SlashScript>() is not null) return;
        GameObject o = new GameObject();
        o.transform.position = transform.position + transform.up * 0.5f;
        o.transform.rotation = transform.rotation;
        o.transform.parent = transform;
        o.layer = 3;
        o.AddComponent<BoxCollider2D>().size *= new Vector2(1.5f, 1);
        o.GetComponent<BoxCollider2D>().isTrigger = true;
        o.AddComponent<SpriteRenderer>();
        o.AddComponent<SlashScript>();
        t[Timer.Parry] = Time.time + 1.5f;
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
        if (Time.time < t[Timer.Dodge]) return;
        MultiplySpeed("dodge",5, 0.15f);
        t[Timer.Dodge] = Time.time + 1f;
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
        currentSpeedMult = Mathf.SmoothDamp(
            current: currentSpeedMult,
            target: speedMult,
            currentVelocity: ref speedMultVelocity,
            smoothTime: 0.08f
        );
        rb.linearVelocity = moveDir * (speed * currentSpeedMult);
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
        if (!controls.Player.Interact.WasPressedThisFrame()) return;
        Interact?.Invoke();
    }
    
    private Dictionary<string, float> multipliers = new ();

    private Dictionary<string, Coroutine> timers = new ();

    public void MultiplySpeed(string key, float mult, float duration = -1)
    {
        multipliers[key] = mult;
        RecalculateSpeed();
        
        if (timers.ContainsKey(key))
        {
            StopCoroutine(timers[key]);
            timers.Remove(key);
        }
        
        if (duration > 0f)
            timers[key] = StartCoroutine(RemoveAfterDuration(key, duration));
    }

    public void RemoveMultiplier(string key)
    {
        if (multipliers.Remove(key))
            RecalculateSpeed();
    }

    private IEnumerator RemoveAfterDuration(string key, float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveMultiplier(key);
    }

    private void RecalculateSpeed()
    {
        speedMult = 1f;
        foreach (var m in multipliers.Values)
            speedMult *= m;
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

    public Vector2 GetLookDirection()
    {
        return lookDirection;
    }

    public int Target()
    {
        return 1;
    }
}
