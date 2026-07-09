
using UnityEngine.InputSystem;

/// <summary>
/// FOR ACTIVE SKILLS ONLY !
/// </summary>
public class MeleeSkill : ActiveSkill
{
    public MeleeSkill(SkillCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
    {
    }   
}