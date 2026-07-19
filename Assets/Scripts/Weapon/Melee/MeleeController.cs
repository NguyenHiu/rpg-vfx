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

    public new List<MeleeSkill> ActiveSkills;

    void Awake()
    {
        // m_weaponLength = Mathf.Abs(m_headTf.localPosition.magnitude * m_sr.transform.localScale.x);
        m_weaponLength = (m_headTf.position - m_sr.transform.position).magnitude;
        InitMeleeSkill();
    }

    protected void InitMeleeSkill()
    {
        Debug.Log("InitMeleeSkill");
        ActiveSkills = new()
        {
            new MeleeBasicAttack(m_basicAttackCfg, m_skillCtrl, m_skillCtrl.Player.InputActions.FindAction(m_basicAttackAction))
        };
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