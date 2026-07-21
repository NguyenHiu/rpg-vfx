
using UnityEngine;

[CreateAssetMenu(fileName = "Slash_Combo_Cfg", menuName = "SO/Skill Configs/Slash Combo")]
public class SlashComboCfg : MeleeCfg
{
    public float SlashSpeed;
    public float SlashAngle;
    public float PiercingSpeed;
    public float PiercingAngle;
    public float ComboDuration;
    public float Radius;
    public Vector2 CenterOffset;
}