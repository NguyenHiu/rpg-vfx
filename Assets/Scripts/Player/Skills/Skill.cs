using UnityEngine;

/// <summary>
/// Standard Skill class for all skills
/// NOTE: This class currently supports Time-Based Skills only!
/// </summary>
public class Skill
{
    protected readonly SkillCfg Cfg;
    public readonly PlayerController Player;
    public bool IsRunning { get; protected set; }
    protected float m_timer = 0;

    public Skill(SkillCfg cfg, PlayerController player)
    {
        Cfg = cfg;
        Player = player;
    }

    public virtual bool CanUse()
    {
        Debug.Log("Skill - m_timer: " + m_timer);
        return m_timer <= 0.01f;
    }
    public virtual void Use()
    {
        IsRunning = true;
    }
    public virtual void FixedUpdate(float dt)
    {
        if (m_timer > 0) m_timer -= dt;
    }
    public virtual void Stop()
    {
        IsRunning = false;
    }
}