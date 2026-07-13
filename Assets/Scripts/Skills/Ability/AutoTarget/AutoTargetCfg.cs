
using UnityEngine;

// NOTE: Passive Skills include 2 types based on how it works (Once or duration)
// TODO: Implement different passive skills 

[CreateAssetMenu(fileName = "AutoTargetConfig", menuName = "SO/Skill Configs/Auto Target")]
public class AutoTargetCfg : SkillCfg
{
    // TODO: Implement different type of auto-target: nearest enemies, lowest health, etc.
    // Current approach: nearest enemies

    public int NTarget;
    public float Radius;
    public LayerMask EnemyLayer;
}