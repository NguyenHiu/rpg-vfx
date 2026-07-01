using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [field: Header("===== Stats =====")]
    [field: SerializeField] public PlayerMode Mode { get; private set; }

    // Walk
    [Header("Walk")]
    [SerializeField] private float m_walkSpeed;
    [Min(0.2f)][field: SerializeField] public float SpeedBuff { get; private set; } // divided by SpeedBuff everywhere -> ensure not too small
    public Action<float> OnSpeedBuffChange;

    [Header("Skills")]
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
    [SerializeField] private float m_battleRadius;
    [SerializeField] private LayerMask m_enemyLayer;
    [SerializeField] private float m_enemyScanDuration;
    private float m_scanTimer;
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

        Mode = PlayerMode.FREE;
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

        if (Mode == PlayerMode.ATTACK) FixedUpdate_AttackMode();
    }

    void FixedUpdate_AttackMode()
    {
        if (FocusTF)
            FacingDir = (FocusTF.position - transform.position).normalized;

        if (m_scanTimer > 0)
        {
            m_scanTimer -= Time.deltaTime;
            return;
        }
        m_scanTimer = m_enemyScanDuration;

        // Find nearest enemy in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, m_battleRadius, m_enemyLayer);
        if (hits.Length == 0)
        {
            if (FocusTF && FocusTF.TryGetComponent<SpriteRenderer>(out var sr))
                sr.color = Color.white;
            FocusTF = null;
            return;
        }
        float nearestVal = -1;
        GameObject nearest = null;
        foreach (var col in hits)
        {
            if (nearestVal == -1 || (col.transform.position - transform.position).magnitude < nearestVal)
            {
                nearestVal = (col.transform.position - transform.position).magnitude;
                nearest = col.gameObject;
            }
        }

        FocusTF = nearest.transform;
        if (nearest.TryGetComponent<SpriteRenderer>(out var sr1))
            sr1.color = Color.green;
        Debug.Log($"Focus on: {nearest.name}");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        float angleStep = 360f / 10;
        Vector3 prevPoint = transform.position + new Vector3(m_battleRadius, 0f, 0f);

        for (int i = 1; i <= 10; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPoint = transform.position + new Vector3(Mathf.Cos(angle) * m_battleRadius, Mathf.Sin(angle) * m_battleRadius, 0f);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
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


    // Debug
    public void EnterAttackMode()
    {
        Mode = PlayerMode.ATTACK;
    }

    public void ExitAttackMode()
    {
        if (FocusTF && FocusTF.TryGetComponent<SpriteRenderer>(out var comp))
        {
            comp.color = Color.white;
        }
        Mode = PlayerMode.FREE;
    }
}
