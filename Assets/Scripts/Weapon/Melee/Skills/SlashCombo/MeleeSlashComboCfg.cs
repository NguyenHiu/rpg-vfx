
using UnityEngine;

[CreateAssetMenu(fileName = "Melee_Slash_Combo_Cfg", menuName = "SO/Skill Configs/Melee Slash Combo")]
public class MeleeSlashComboCfg : MeleeCfg
{
    public float SlashSpeed;
    public float SlashAngle;
    public float PiercingSpeed;
    public float PiercingAngle;
    public float ComboDuration;
    public float Radius;
    public Vector2 CenterOffset;
}