using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerMode
{
    FREE,
    ATTACK,
}

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputActionAsset m_inputActions;
    [field: SerializeField] public Rigidbody2D Rb { get; private set; }

    [Header("===== Stats =====")]

    // Walk
    [Header("Walk")]
    [SerializeField] private float m_walkSpeed;
    [Min(0.2f)][field: SerializeField] public float SpeedBuff { get; private set; } // divided by SpeedBuff everywhere -> ensure not too small
    public Action<float> OnSpeedBuffChange;

    // Dash
    [Header("Dash")]
    [SerializeField] private float m_dashSpeed;
    [field: SerializeField] public GhostTrailController GhostTrailCtrl { get; private set; }
    [field: SerializeField] public float DashCooldown { get; private set; }
    private float m_dashTimer;
    [field: SerializeField] public float DashTime { get; private set; }
    [field: SerializeField] public bool IsDashing { get; private set; }
    [field: SerializeField] public Vector2 DashDir { get; private set; }

    // Attack
    [field: Header("Attack")]
    [field: SerializeField] public Transform FocusTF { get; private set; }
    [field: SerializeField] public Vector2 FacingDir { get; private set; }
    [field: SerializeField] public bool IsAttacking { get; private set; }
    [field: SerializeField] public float AttackCooldown { get; private set; }
    private float m_attackTimer;

    [field: Header("Hand")]
    [field: SerializeField] public WeaponController Weapon { get; private set; }

    // Inputs
    [Header("Inputs")]
    [SerializeField] private InputAction m_moveAction;
    [SerializeField] private InputAction m_dashAction;
    [SerializeField] private InputAction m_attackAction;
    [field: SerializeField] public Vector2 MoveVal { get; private set; }
    [field: SerializeField] public bool PressDashThisFrame { get; private set; }
    [field: SerializeField] public bool PressAttackThisFrame { get; private set; }

    void OnEnable()
    {
        m_inputActions.FindActionMap("Player").Enable();
    }

    void OnDisable()
    {
        m_inputActions.FindActionMap("Player").Disable();
    }

    void Start()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_dashAction = InputSystem.actions.FindAction("Dash");
        m_attackAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        if (m_dashTimer >= 0) m_dashTimer -= Time.deltaTime;
        if (m_attackTimer >= 0) m_attackTimer -= Time.deltaTime;
        MoveVal = m_moveAction.ReadValue<Vector2>();
        PressDashThisFrame = m_dashTimer < 0f && m_dashAction.IsPressed();
        PressAttackThisFrame = m_attackTimer < 0f && m_attackAction.IsPressed();
    }

    void FixedUpdate()
    {
        var dir = IsDashing ? DashDir : MoveVal.normalized;
        Rb.linearVelocity = dir * GetSpeed();

        if (Rb.linearVelocity != Vector2.zero) FacingDir = Rb.linearVelocity.normalized;
    }

    public float GetSpeed()
    {
        var baseSpeed = IsDashing ? m_dashSpeed : m_walkSpeed;
        return baseSpeed * SpeedBuff;
    }

    public void SetSpeedBuff(float newVal)
    {
        SpeedBuff = newVal;
        OnSpeedBuffChange?.Invoke(SpeedBuff);
    }

    public void ResetDashTimer()
    {
        m_dashTimer = DashCooldown;
    }

    public void ResetAttackTimer()
    {
        m_attackTimer = AttackCooldown;
    }

    public bool IsAbleToDash()
    {
        return !IsDashing && !IsAttacking && PressDashThisFrame;
    }

    public void SetDashing(bool val)
    {
        IsDashing = val;
        if (val) DashDir = Rb.linearVelocity.normalized;
    }

    public bool IsAbleToAttack()
    {
        return !IsDashing && !IsAttacking && PressAttackThisFrame;
    }

    public void SetAttacking(bool val)
    {
        IsAttacking = val;
    }
}
