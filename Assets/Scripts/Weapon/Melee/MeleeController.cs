using System.Collections.Generic;
using UnityEngine;

// Includes: Sword, Axe
public class MeleeController : WeaponController, IBasicAttack
{
    [Header("Requirements")]
    [Header("> Basic Attack")]
    [SerializeField] protected MeleeBasicAttackCfg m_basicAttackCfg;
    public MeleeBasicAttackCfg BasicAttackCfg => m_basicAttackCfg;
    [SerializeField] protected string m_basicAttackAction;
    [SerializeField] protected Transform m_headTf;
    [SerializeField] protected SpriteRenderer m_slashSR;
    public SpriteRenderer SlashSR => m_slashSR;

    [Header("> Pierce Attack")]
    [SerializeField] protected MeleePierceAttackCfg m_pierceCfg;
    public MeleePierceAttackCfg PierceCfg => m_pierceCfg;
    [SerializeField] protected string m_pierceAction;
    [SerializeField] protected int m_piercingLayer;
    public int PiercingLayer => m_piercingLayer;

    [Header("> Slash combo")]
    [SerializeField] protected MeleeSlashComboCfg m_slashComboCfg;
    public MeleeSlashComboCfg SlashComboCfg => m_slashComboCfg;

    [Header("View Only")]
    [SerializeField] protected float m_weaponLength;
    public float WeaponLength => m_weaponLength;

    void Awake()
    {
        m_weaponLength = (m_headTf.position - m_sr.transform.position).magnitude;
        InitMeleeSkill();
    }

    protected void InitMeleeSkill()
    {
        m_activeSkills = new()
        {
            // new MeleeBasicAttack(m_basicAttackCfg, m_skillCtrl, m_skillCtrl.Player.InputActions.FindAction(m_basicAttackAction)),
            new MeleePierceAttack(m_pierceCfg, m_skillCtrl, m_skillCtrl.Player.InputActions.FindAction(m_pierceAction)),
            new MeleeSlashCombo(m_slashComboCfg, m_skillCtrl, m_skillCtrl.Player.InputActions.FindAction(m_basicAttackAction)),
        };
    }

    public MeleeSkill GetSkill(MELEE_SKILL type)
    {
        foreach (var skill in m_activeSkills)
        {
            var meleeSkill = skill as MeleeSkill;
            if (meleeSkill.Type == type) return meleeSkill;
        }
        return null;
    }
}