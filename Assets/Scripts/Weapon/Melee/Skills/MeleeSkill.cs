
using UnityEngine.InputSystem;

public enum MELEE_SKILL
{
    BASIC_ATTACK,
    PIERCE_ATTACK,
    SLASH_COMBO,
}

public class MeleeCfg : SkillCfg
{
    public MELEE_SKILL Type;
}


public class MeleeSkill : ActiveSkill
{
    protected new MeleeController Weapon;
    protected readonly new MeleeCfg Cfg;
    public MELEE_SKILL Type => Cfg.Type;

    public MeleeSkill(MeleeCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
    {
        Weapon = (MeleeController)skillCtrl.Weapon;
        Cfg = cfg;
    }
}