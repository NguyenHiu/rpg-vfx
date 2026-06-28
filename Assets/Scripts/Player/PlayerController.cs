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
    public InputActionAsset InputActions;
    public Rigidbody2D Rb;

    [Header("===== Stats =====")]

    // Walk
    [Header("Walk")]
    [SerializeField] private float m_walkSpeed;
    [Min(0.2f)][SerializeField] private float m_speedBuff; // divided by SpeedBuff everywhere -> ensure not too small
    public float SpeedBuff => m_speedBuff;
    public Action<float> OnSpeedBuffChange;

    // Dash
    [Header("Dash")]
    [SerializeField] private float m_dashSpeed;
    public GhostTrailController GhostTrailCtrl;
    public float DashCooldown;
    private float m_dashTimer;
    public float DashTime;
    public bool IsDashing;
    public Vector2 DashDir;

    // Attack
    [Header("Attack")]
    public Transform FocusTF;
    public Vector2 FacingDir = new(0, -1);
    public bool IsAttacking;
    public float AttackCooldown;
    private float m_attackTimer;

    [Header("Hand")]
    public WeaponController Weapon;

    // Inputs
    [Header("Inputs")]
    [SerializeField] private InputAction m_moveAction;
    [SerializeField] private InputAction m_dashAction;
    [SerializeField] private InputAction m_attackAction;
    public Vector2 MoveVal;
    public bool PressDashThisFrame;
    public bool PressAttackThisFrame;

    void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
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
        m_speedBuff = newVal;
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

    public bool IsAbleToAttack()
    {
        return !IsDashing && !IsAttacking && PressAttackThisFrame;
    }
}
