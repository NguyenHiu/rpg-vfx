

public class PassiveSkill : Skill
{
    public bool IsTriggered {get; protected set; }

    public PassiveSkill(SkillCfg cfg, SkillController skillCtrl) : base(cfg, skillCtrl)
    {

    }

    public virtual void Trigger()
    {
        IsTriggered = true;
    }

    public override void Activate()
    {
        base.Activate();
        IsTriggered = false;
    }
}


