using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct ActiveSkill
{
    public InputAction Action;
    public Skill Skill;
}

[Serializable]
public class SkillController
{
    [Header("Skills Controller")]
    [SerializeField] private PlayerController m_player;
    [SerializeField] private List<ActiveSkill> m_activeSkills;
    [SerializeField] private Skill m_lastSkill;

    [Header("Dash")]
    [SerializeField] private DashConfig m_dashCfg;
    [SerializeField] private SpriteRenderer m_trailSample;
    [SerializeField] private Transform m_trailParent;


    public SkillController()
    {
        InitActiveSkills();
    }

    public void FixedUpdate(float dt)
    {
        UpdateActiveSkills(dt);
    }

    private void InitActiveSkills()
    {
        m_activeSkills = new()
        {
            new ActiveSkill()
            {
                Action = InputSystem.actions.FindAction("Dash"),
                Skill = new DashSkill(m_dashCfg, m_player, m_trailSample, m_trailParent)
            }
        };
    }

    private void UpdateActiveSkills(float dt)
    {
        if (m_lastSkill == null || !m_lastSkill.IsRunning)
        {
            foreach (var skill in m_activeSkills)
            {
                if (skill.Skill.CanUse() && skill.Action.IsPressed())
                    m_lastSkill = skill.Skill;
            }
        }

        if (m_lastSkill == null) return;

        // m_lastSkill.FixedUpdate(dt);
        Debug.Log("Last Skill");
    }
}