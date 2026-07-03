using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct ActiveSkill
{
    public InputAction Action;
    public Skill Skill;
}

public class SkillController : MonoBehaviour
{
    [Header("Skills Controller")]
    public PlayerController Player;
    public WeaponController Weapon;
    private List<ActiveSkill> m_activeSkills;
    private Skill m_lastSkill;

    [Header("Dash")]
    [SerializeField] private string m_dashInputActionName;
    [SerializeField] private DashConfig m_dashCfg;
    [SerializeField] private SpriteRenderer m_trailSample;
    [SerializeField] private Transform m_trailParent;

    [Header("Basic Attack")]
    [SerializeField] private string m_attackInputActionName;
    [SerializeField] private BasicAttackCfg m_basicAttackCfg;

    void Awake()
    {
        m_activeSkills = new()
        {
            new ActiveSkill()
            {
                Action = InputSystem.actions.FindAction(m_dashInputActionName),
                Skill = new DashSkill(m_dashCfg, this, m_trailSample, m_trailParent)
            },
            new ActiveSkill()
            {
                Action = InputSystem.actions.FindAction(m_attackInputActionName),
                Skill = new BasicAttack(m_basicAttackCfg, this)
            }
        };
        m_lastSkill = null;
    }

    public void ManualUpdate(float dt)
    {
        foreach (var skill in m_activeSkills)
        {
            skill.Skill.Update(dt);
        }
    }

    public void ManualFixedUpdate(float dt, PlayerContext context)
    {
        bool isUseSkill = false;
        if (m_lastSkill == null || !m_lastSkill.IsRunning)
        {
            foreach (var skill in m_activeSkills)
            {
                if (skill.Action.IsPressed() && skill.Skill.CanUse())
                {
                    m_lastSkill = skill.Skill;
                    isUseSkill = true;
                }
            }
        }

        if (m_lastSkill == null) return;

        if (isUseSkill)
        {
            m_lastSkill.Use();
            Debug.Log("[SkillController] Use new skill");
        }
        else if (m_lastSkill.IsRunning)
        {
            m_lastSkill.FixedUpdate(dt, context);
            Debug.Log("[SkillController] Update Running Skill");
        }
        
    }
}