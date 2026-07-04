
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveSkill : Skill
{
    public readonly InputAction Action;

    public ActiveSkill(SkillCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl)
    {
        Action = action;
    }

    public virtual bool IsTriggering()
    {
        return Action.IsPressed() && Available();
    }

    public override void Activate()
    {
        Debug.Log($"Activate Skill '{Cfg.Name}'");
        base.Activate();
    }
}