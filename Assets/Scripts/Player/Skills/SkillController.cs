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

public class SkillController : MonoBehaviour
{
    [Header("Skills Controller")]
    [SerializeField] private PlayerController m_player;
    private List<ActiveSkill> m_activeSkills;
    private Skill m_lastSkill;

    [Header("Dash")]
    [SerializeField] private DashConfig m_dashCfg;
    [SerializeField] private SpriteRenderer m_trailSample;
    [SerializeField] private Transform m_trailParent;


    public SkillController()
    {
        m_activeSkills = new();
        // {
        //     new ActiveSkill()
        //     {
        //         Action = InputSystem.actions.FindAction("Dash"),
        //         Skill = new DashSkill(m_dashCfg, m_player, m_trailSample, m_trailParent)
        //     }
        // };
    }

    public void ManualUpdate(float dt, PlayerContext context)
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

        m_lastSkill.FixedUpdate(dt, context);
        Debug.Log("Last Skill");
    }
}