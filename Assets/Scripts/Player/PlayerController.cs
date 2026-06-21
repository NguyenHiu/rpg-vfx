using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public InputActionAsset InputActions;
    public Rigidbody2D Rb;

    [Header("Stats")]
    // Walk
    [SerializeField] private float walkSpeed;
    [Min(0.2f)][SerializeField] private float speedBuff; // divided by SpeedBuff everywhere -> ensure not too small
    public float SpeedBuff => speedBuff;
    public Action<float> OnSpeedBuffChange;
    // Dash
    [SerializeField] private float dashSpeed;
    public GhostTrailController GhostTrailCtrl;
    public float DashCooldown;
    private float dashTimer;
    public float DashTime;
    public bool IsDashing;
    public Vector2 DashDir;

    [Header("Hand")]
    public WeaponController Weapon;
    public Vector2 FacingDir = new(0, -1);
    public Animator SlashAnim;
    public bool IsAttacking;

    // Inputs
    private InputAction m_moveAction;
    private InputAction m_dashAction;
    private InputAction m_attackAction;
    public Vector2 MoveVal;
    public bool PressDashThisFrame;
    public bool AttackThisFrame;

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
        if (dashTimer >= 0) dashTimer -= Time.deltaTime;
        MoveVal = m_moveAction.ReadValue<Vector2>();
        PressDashThisFrame = dashTimer < 0f && m_dashAction.IsPressed();
        AttackThisFrame = !IsAttacking && m_attackAction.IsPressed();
    }

    void FixedUpdate()
    {
        var dir = IsDashing ? DashDir : MoveVal.normalized;
        Rb.linearVelocity = dir * GetSpeed();

        if (Rb.linearVelocity != Vector2.zero) FacingDir = Rb.linearVelocity.normalized;

        if (AttackThisFrame && !IsAttacking)
            SlashAnim.SetTrigger("Attack");
    }

    public float GetSpeed()
    {
        var baseSpeed = IsDashing ? dashSpeed : walkSpeed;
        return baseSpeed * SpeedBuff;
    }

    public void SetSpeedBuff(float newVal)
    {
        speedBuff = newVal;
        OnSpeedBuffChange?.Invoke(SpeedBuff);
    }

    public void ResetDashTimer()
    {
        dashTimer = DashCooldown;
    }
}
