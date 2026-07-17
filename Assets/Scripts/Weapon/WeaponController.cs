using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] protected SpriteRenderer m_sr;
    public SpriteRenderer SR => m_sr;
    [SerializeField] protected WeaponIdleAnimCfg m_idleAnimCfg;
    public WeaponIdleAnimCfg IdleAnimCfg => m_idleAnimCfg;
    [SerializeField] protected SkillController m_skillCtrl;
    public SkillController SkillCtrl => m_skillCtrl;

    [Header("View Only")]
    [SerializeField] protected WeaponAnimController m_animCtrl;
    public WeaponAnimController AnimCtrl => m_animCtrl;
    
    public List<ActiveSkill> ActiveSkills;
}