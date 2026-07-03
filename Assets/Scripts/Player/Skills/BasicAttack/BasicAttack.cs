using UnityEngine;

public class BasicAttack : Skill
{
    // TODO: Use Cfg.Speed
    protected new BasicAttackCfg Cfg;

    public BasicAttack(BasicAttackCfg cfg, SkillController skillCtrl) : base(cfg, skillCtrl)
    {
        Cfg = cfg;
    }

    public override void Use()
    {
        base.Use();
        Debug.Log("[BasicAttack] - Change state to ATTACK");
        Weapon.ChangeState(WState.ATTACK, CompleteAttack);
    }

    public void CompleteAttack()
    {
        Weapon.ChangeState(WState.IDLE);
        Stop();
    }
}