

public class BasicAttack : Skill
{
    protected new BasicAttackCfg Cfg;

    public BasicAttack(BasicAttackCfg cfg, SkillController skillCtrl) : base(cfg, skillCtrl)
    {
        Cfg = cfg;
    }

    public override void Use()
    {
        base.Use();
        Weapon.ChangeState(WState.ATTACK, CompleteAttack);
    }

    public void CompleteAttack()
    {
        Weapon.ChangeState(WState.IDLE);
    }
}