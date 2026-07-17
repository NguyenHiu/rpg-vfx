using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SkillController : MonoBehaviour
{
    [Header("Requirements")]
    public PlayerController Player;
    public WeaponController Weapon;

    [Header("View Only")]
    private List<ActiveSkill> m_activeSkills;
    private List<PassiveSkill> m_passiveSkills;
    private Skill m_lastSkill;

    [Header("Player Based Skills")]
    [Header("== Active Skills")]
    [Header("> Dash")]
    [SerializeField] private string m_dashInputActionName;
    [SerializeField] private DashConfig m_dashCfg;
    [SerializeField] private SpriteRenderer m_trailSample;
    [SerializeField] private SortingGroup m_playerSortingGroup;
    [SerializeField] private Transform m_trailParent;

    [Header("== Passive Skills")]
    [Header("> Auto Target")]
    [SerializeField] private AutoTargetCfg m_autoTargetCfg;
    [SerializeField] private Transform m_centerTf;

    void Awake()
    {
        m_passiveSkills = new()
        {
            new AutoTargetSkill(m_autoTargetCfg, this, m_centerTf)
        };
        m_activeSkills = new()
        {
            new DashSkill(m_dashCfg, this, InputSystem.actions.FindAction(m_dashInputActionName), m_trailSample, m_playerSortingGroup, m_trailParent),
        };
        m_lastSkill = null;
    }

    public void ManualUpdate(float dt)
    {
        foreach (var skill in m_passiveSkills)
            skill.Update(dt);
        foreach (var skill in m_activeSkills)
            skill.Update(dt);
        if (Weapon.ActiveSkills != null)
        {
            foreach (var skill in Weapon.ActiveSkills)
                skill.Update(dt);
        }
    }

    // public void ManualFixedUpdate(float dt, PlayerContext context)
    // {
    //     FixedUpdate_PassiveSkills(dt, context);
    //     FixedUpdate_ActiveSkills(dt, context);
    // }

    public void FixedUpdate_PassiveSkills(float dt, PlayerContext context)
    {
        // Allow inf passive skills activate at once
        foreach (var skill in m_passiveSkills)
        {
            if (skill.MeetCondition())
                skill.Activate();
            skill.FixedUpdate(dt, context);
        }
    }

    public void FixedUpdate_ActiveSkills(float dt, PlayerContext context)
    {
        bool isUseSkill = false;
        if (m_lastSkill == null || !m_lastSkill.IsRunning)
        {
            foreach (var skill in m_activeSkills)
            {
                if (skill.IsTriggering())
                {
                    m_lastSkill = skill;
                    isUseSkill = true;
                    break;
                }
            }

            if (!isUseSkill && Weapon.ActiveSkills != null)
            {
                foreach (var skill in Weapon.ActiveSkills)
                {
                    if (skill.IsTriggering())
                    {
                        m_lastSkill = skill;
                        isUseSkill = true;
                        break;
                    }
                }
            }
        }

        if (m_lastSkill == null) return;

        if (isUseSkill)
        {
            m_lastSkill.Activate();
            // Debug.Log("[SkillController] Use new skill");
        }
        else if (m_lastSkill.IsRunning)
        {
            m_lastSkill.FixedUpdate(dt, context);
            // Debug.Log("[SkillController] Update Running Skill");
        }
    }

    void OnDrawGizmos()
    {
        if (m_passiveSkills != null)
            foreach (var skill in m_passiveSkills)
                skill?.OnDrawGizmos();
        if (m_activeSkills != null)
            foreach (var skill in m_activeSkills)
                skill?.OnDrawGizmos();
    }

    public Skill GetSkill(string name)
    {
        if (m_activeSkills != null)
            foreach (var skill in m_activeSkills)
            {
                if (skill != null && skill.GetCfg().Name == name) return skill;
            }

        if (m_passiveSkills != null)
            foreach (var skill in m_passiveSkills)
            {
                if (skill != null && skill.GetCfg().Name == name)
                    return skill;
            }

        return null;
    }
}