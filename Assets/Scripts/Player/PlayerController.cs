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

[Serializable]
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
    public InputActionAsset InputActions => m_inputActions;
    [SerializeField] private Rigidbody2D m_rb;
    public Rigidbody2D RB => m_rb;
    [SerializeField] private PlayerAnimController m_anim;
    public PlayerAnimController Anim;
    [SerializeField] private SpriteRenderer m_sr;
    public SpriteRenderer SR => m_sr;
    [SerializeField] private Collider2D m_moveCollider;
    public Collider2D MoveCollider => m_moveCollider;

    [SerializeField] private PlayerMode m_mode;
    public PlayerMode Mode => m_mode;
    [SerializeField] private PlayerContext m_ctx;
    [SerializeField] private PlayerStats m_stats;
    [SerializeField] private SkillController m_skillCtrl;
    [SerializeField] private Vector2 m_facingDir;
    public Vector2 FacingDir => m_facingDir;
    [SerializeField] private WeaponAnimController m_weapon;
    public WeaponAnimController Weapon => m_weapon;

    // Inputs
    [Header("Inputs")]
    [SerializeField] private InputAction m_moveAction;
    [SerializeField] private Vector2 m_moveVal;
    public Vector2 MoveVal => m_moveVal;

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
        m_moveVal = m_moveAction.ReadValue<Vector2>();
        m_skillCtrl.ManualUpdate(Time.deltaTime);
    }

    void FixedUpdate()
    {
        m_ctx.Direction = MoveVal.normalized;
        m_ctx.Speed = m_stats.GetWalkSpeed();
        m_ctx.Targets.Clear();
        m_skillCtrl.FixedUpdate_PassiveSkills(Time.fixedDeltaTime, m_ctx);

        // AUTO TARGET in BATTLE MODE
        if (m_mode == PlayerMode.ATTACK && m_ctx.Targets.Count != 0)
            m_facingDir = (m_ctx.Targets[0].transform.position - transform.position).normalized;
        else if (m_rb.linearVelocity != Vector2.zero)
            m_facingDir = m_rb.linearVelocity.normalized;

        m_skillCtrl.FixedUpdate_ActiveSkills(Time.fixedDeltaTime, m_ctx);
        m_rb.linearVelocity = m_ctx.Direction * m_ctx.Speed;
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public void ChangeMode(PlayerMode mode)
    {
        m_mode = mode;
    }
#endif
}
