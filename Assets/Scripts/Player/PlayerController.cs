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

public class PlayerContext
{
    public Vector2 Direction;
    public float Speed;
    public List<GameObject> Targets;
}

[RequireComponent(typeof(SkillController))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputActionAsset m_inputActions;
    [field: SerializeField] public Rigidbody2D Rb { get; private set; }
    [field: SerializeField] public PlayerAnimController Anim;
    [field: SerializeField] public SpriteRenderer SR;

    [Header("===== Stats =====")]
    [SerializeField] private PlayerMode m_mode;
    public PlayerMode Mode => m_mode;
    [SerializeField] private PlayerContext m_ctx;
    [SerializeField] private PlayerStats m_stats;
    [SerializeField] private SkillController m_skillCtrl;

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
        m_attackAction = InputSystem.actions.FindAction("Attack");

        m_mode = PlayerMode.FREE;
        m_skillCtrl = GetComponent<SkillController>();
        m_stats = GetComponent<PlayerStats>();
        m_ctx = new();
    }

    void Update()
    {
        if (m_attackTimer >= 0) m_attackTimer -= Time.deltaTime;
        MoveVal = m_moveAction.ReadValue<Vector2>();
        PressAttackThisFrame = m_attackTimer < 0f && m_attackAction.IsPressed();
        m_skillCtrl.ManualUpdate(Time.deltaTime);
    }

    void FixedUpdate()
    {
        m_ctx.Direction = MoveVal.normalized;
        m_ctx.Speed = m_stats.GetWalkSpeed();
        m_skillCtrl.ManualFixedUpdate(Time.deltaTime, m_ctx);
        Rb.linearVelocity = m_ctx.Direction * m_ctx.Speed;
        if (Rb.linearVelocity != Vector2.zero) FacingDir = Rb.linearVelocity.normalized;
    }

    public void ResetAttackTimer()
    {
        m_attackTimer = AttackCooldown;
    }
    
    public void SetAttacking(bool val)
    {
        IsAttacking = val;
    }


    // Debug
    public void EnterAttackMode()
    {
        m_mode = PlayerMode.ATTACK;
    }

    public void ExitAttackMode()
    {
        if (FocusTF && FocusTF.TryGetComponent<SpriteRenderer>(out var comp))
        {
            comp.color = Color.white;
        }
        m_mode = PlayerMode.FREE;
    }
}
