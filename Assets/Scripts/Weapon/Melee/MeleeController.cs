using System.Collections.Generic;
using UnityEngine;

// Includes: Sword, Axe
public class MeleeController : WeaponController, IBasicAttack
{
    [Header("Requirements")]
    [SerializeField] protected MeleeBasicAttackCfg m_basicAttackCfg;
    public MeleeBasicAttackCfg BasicAttackCfg => m_basicAttackCfg;
    [SerializeField] protected string m_basicAttackAction;
    [SerializeField] protected Transform m_headTf;
    [SerializeField] protected Animator m_slashAnim;
    public Animator SlashAnim => m_slashAnim;

    [Header("View Only")]
    [SerializeField] protected float m_weaponLength;
    public float WeaponLength => m_weaponLength;
    [SerializeField] protected PolygonCollider2D m_basicAttackCollider;
    public PolygonCollider2D BasicAttackCollider => m_basicAttackCollider;

    void Awake()
    {
        InitBasicAttack();
        InitMeleeSkill();
    }

    protected void InitBasicAttack()
    {
        // m_weaponLength = Mathf.Abs(m_headTf.localPosition.magnitude * m_sr.transform.localScale.x);
        m_weaponLength = (m_headTf.position - m_sr.transform.position).magnitude;
        m_basicAttackCollider.gameObject.SetActive(false);

        // Create the pizza collision xD
        var rad = m_basicAttackCfg.Angle * Mathf.Deg2Rad;
        var sinVal = Mathf.Sin(rad);
        var cosVal = Mathf.Cos(rad);
        Vector2[] points = new Vector2[]
        {
            new(0, 0),
            new(-sinVal*m_weaponLength, cosVal*m_weaponLength),
            new(0, m_weaponLength),
            new(sinVal*m_weaponLength, cosVal*m_weaponLength)
        };
        m_basicAttackCollider.SetPath(0, points);
    }

    protected void InitMeleeSkill()
    {
        ActiveSkills = new()
        {
            new MeleeBasicAttack(m_basicAttackCfg, m_skillCtrl, m_skillCtrl.Player.InputActions.FindAction(m_basicAttackAction))
        };
    }
}