

public class PassiveSkill : Skill
{

    public PassiveSkill(SkillCfg cfg, SkillController skillCtrl) : base(cfg, skillCtrl)
    {

    }

    public virtual bool MeetCondition()
    {
        return false;
    }
}


