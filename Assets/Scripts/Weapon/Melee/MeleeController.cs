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
    [SerializeField] protected PolygonCollider2D m_pierceAttackCollider;
    public PolygonCollider2D PierceAttackCollider => m_pierceAttackCollider;

    public new List<MeleeSkill> ActiveSkills;

    void Awake()
    {
        // m_weaponLength = Mathf.Abs(m_headTf.localPosition.magnitude * m_sr.transform.localScale.x);
        m_weaponLength = (m_headTf.position - m_sr.transform.position).magnitude;
        InitBasicAttack();
        InitMeleeSkill();
    }

    protected void InitMeleeSkill()
    {
        ActiveSkills = new()
        {
            new MeleeBasicAttack(m_basicAttackCfg, m_skillCtrl, m_skillCtrl.Player.InputActions.FindAction(m_basicAttackAction))
        };
    }

    protected void InitBasicAttack()
    {
        m_basicAttackCollider.gameObject.SetActive(false);
    }

    protected void InitPierceAttack()
    {
        
    }

    public MeleeSkill GetSkill(MELEE_SKILL type)
    {
        foreach(var skill in ActiveSkills)
        {
            if (skill.Type == type) return skill;
        }
        return null;
    }
}