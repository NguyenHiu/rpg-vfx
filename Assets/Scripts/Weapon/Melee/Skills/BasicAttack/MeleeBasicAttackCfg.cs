using UnityEngine;

[CreateAssetMenu(fileName="MeleeBasicAttackConfig", menuName ="SO/Skill Configs/Melee Basic Attack")]
public class MeleeBasicAttackCfg : SkillCfg
{
    public float Speed;
    public float Angle;
    public float Radius;
    public Vector2 CenterOffset;
}