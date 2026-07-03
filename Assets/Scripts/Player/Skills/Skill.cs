using System;
using UnityEngine;

/// <summary>
/// Standard Skill class for all skills
/// NOTE: This class currently supports Time-Based Skills only!
/// </summary>
[Serializable]
public class Skill
{
    [SerializeField] protected readonly SkillCfg Cfg;
    [SerializeField] protected SkillController SkillCtrl;
    protected PlayerController Player;
    protected WeaponController Weapon;
    public bool IsRunning { get; protected set; }
    protected float m_timer = 0;

    public Skill(SkillCfg cfg, SkillController skillCtrl)
    {
        Cfg = cfg;
        SkillCtrl = skillCtrl;
        Player = SkillCtrl.Player;
        Weapon = SkillCtrl.Weapon;
    }

    public virtual bool Available()
    {
        return m_timer <= 0.01f;
    }
    public virtual void Activate()
    {
        IsRunning = true;
        m_timer = Cfg.Cooldown;
    }
    public virtual void Cancel()
    {
        IsRunning = false;
    }

    // DON'T ASK ME WHY
    // I DON'T EVEN KNOW TOO
    // Like, I want the most flexibility (I mean as I expected, I'm really suck xD)
    // in skill management, so I decide to manual update and fixedUpdate for better and deterministic game logic (I think so) 

    public virtual void Update(float dt)
    {
        if (m_timer > 0) m_timer -= dt;
    }
    public virtual void FixedUpdate(float dt, PlayerContext context)
    {
    }

    #if UNITY_EDITOR || DEVELOPMENT_BUILD
    public virtual void OnDrawGizmos()
    {
    }
    #endif
}