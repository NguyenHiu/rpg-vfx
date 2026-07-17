using UnityEngine.InputSystem;
using UnityEngine;

public interface IBasicAttack
{
}

public class MeleeBasicAttack : MeleeSkill
{
    // TODO: Use Cfg.Speed
    protected new MeleeBasicAttackCfg Cfg;
    protected new MeleeController Weapon;

    public MeleeBasicAttack(MeleeBasicAttackCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
    {
        Weapon = (MeleeController) skillCtrl.Weapon;
        Cfg = cfg;
    }

    public override void Activate()
    {
        base.Activate();
        Weapon.AnimCtrl.ChangeState(WState.ATTACK, CompleteAttack);
    }

    public void CompleteAttack()
    {
        Weapon.AnimCtrl.ChangeState(WState.IDLE);
        Cancel();
    }
}