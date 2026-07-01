using UnityEngine;

[CreateAssetMenu(fileName="DashCfg", menuName ="ScriptableObject/SkillConfig/DashConfig")]
public class DashCfg : SkillCfg
{
    public float Speed;
    public float Duration;

    [Header("Trail")]
    public int NGhostTrails;
    public float TrailLifetime;
    public float TrailSpawnDelta;
    public GameObject TrailPrefab;
}