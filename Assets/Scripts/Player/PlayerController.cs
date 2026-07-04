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

    public PlayerContext()
    {
        Targets = new();
    }
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
    [field: Header("Attack  ")]
    [field: SerializeField] public Vector2 FacingDir { get; private set; }

    [field: Header("Hand")]
    [field: SerializeField] public WeaponController Weapon { get; private set; }

    // Inputs
    [Header("Inputs")]
    [SerializeField] private InputAction m_moveAction;
    [field: SerializeField] public Vector2 MoveVal { get; private set; }

    void OnEnable()
    {
        if (m_inputActions)
            m_inputActions.FindActionMap("Player").Enable();
    }

    void OnDisable()
    {
        if (m_inputActions)
            m_inputActions.FindActionMap("Player").Disable();
    }

    void Awake()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");

        m_mode = PlayerMode.FREE;
        m_skillCtrl = GetComponent<SkillController>();
        m_stats = GetComponent<PlayerStats>();
        m_ctx = new();
    }

    void Update()
    {
        MoveVal = m_moveAction.ReadValue<Vector2>();
        m_skillCtrl.ManualUpdate(Time.deltaTime);
    }

    void FixedUpdate()
    {
        m_ctx.Direction = MoveVal.normalized;
        m_ctx.Speed = m_stats.GetWalkSpeed();
        m_skillCtrl.ManualFixedUpdate(Time.fixedDeltaTime, m_ctx);
        Rb.linearVelocity = m_ctx.Direction * m_ctx.Speed;
        if (Rb.linearVelocity != Vector2.zero) FacingDir = Rb.linearVelocity.normalized;
    }

    #if UNITY_EDITOR || DEVELOPMENT_BUILD
    public void ChangeMode(PlayerMode mode)
    {
        m_mode = mode;
    }
    #endif
}
