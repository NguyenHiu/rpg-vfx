
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
        // Debug.Log("Action.IsPressed(): " + Action.IsPressed());
        // Debug.Log("Available(): " + Available());
        return Action.IsPressed() && Available();
    }

    public override void Activate()
    {
        Debug.Log($"Activate Skill '{Cfg.Name}'");
        base.Activate();
    }
}