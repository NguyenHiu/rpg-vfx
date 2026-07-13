using UnityEngine.InputSystem;

public interface IBasicAttack
{
}

public class BasicAttack : MeleeSkill
{
    // TODO: Use Cfg.Speed
    protected new BasicAttackCfg Cfg;

    public BasicAttack(BasicAttackCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
    {
        Cfg = cfg;
    }

    public override void Activate()
    {
        base.Activate();
        // Debug.Log("[BasicAttack] - Change state to ATTACK");
        Weapon.ChangeState(WState.ATTACK, CompleteAttack);
    }

    public void CompleteAttack()
    {
        Weapon.ChangeState(WState.IDLE);
        Cancel();
    }
}