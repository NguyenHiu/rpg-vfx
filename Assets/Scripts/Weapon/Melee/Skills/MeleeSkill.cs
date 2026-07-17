
using UnityEngine.InputSystem;

public enum MELEE_SKILL
{
    BASIC_ATTACK,
    PIERCE_ATTACK
}

public class MeleeCfg : SkillCfg
{
    public MELEE_SKILL Type;
}


public class MeleeSkill : ActiveSkill
{
    protected readonly new MeleeCfg Cfg;

    public MeleeSkill(MeleeCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
    {
        Cfg = cfg;
    }
}