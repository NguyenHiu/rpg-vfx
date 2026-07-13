
using UnityEngine;
using UnityEngine.InputSystem;

public class PierceAttack : ActiveSkill
{
    protected new PierceAttackCfg Cfg;
    protected float m_pierceTimer;
    protected Vector2 m_piercingDir;
    protected AutoTargetSkill m_targetSkill;

    public PierceAttack(PierceAttackCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
    {
        Cfg = cfg;

        m_targetSkill = (AutoTargetSkill) SkillCtrl.GetSkill("Auto Target");
    }

    public override void Activate()
    {
        base.Activate();

        m_pierceTimer = Cfg.Duration;
        Player.CD2D.isTrigger = true;
        m_piercingDir = Player.FacingDir;
        m_targetSkill?.SetEnable(false);
        Debug.Log("[PierceAttack] Activate");
    }

    public override bool Available()
    {
        return base.Available();
    }

    public override bool IsTriggering()
    {
        return base.IsTriggering();
    }

    public override void Update(float dt)
    {
        base.Update(dt);
    }

    public override void FixedUpdate(float dt, PlayerContext context)
    {
        base.FixedUpdate(dt, context);

        m_pierceTimer -= dt;
        if (m_pierceTimer <= 0)
        {
            Cancel();
            return;
        }

        context.Direction = m_piercingDir;
        context.Speed = Cfg.Speed;
    }

    public override void Cancel()
    {
        base.Cancel();
        Player.CD2D.isTrigger = false;
        m_targetSkill?.SetEnable(true);
    }
}