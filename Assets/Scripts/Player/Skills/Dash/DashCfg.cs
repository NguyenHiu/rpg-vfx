using UnityEngine;

[CreateAssetMenu(fileName="DashConfig", menuName ="ScriptableObject/SkillConfig/DashConfig")]
public class DashConfig : SkillCfg
{
    public float Speed;
    public float Duration; 

    [Header("Trail")]
    public int NGhostTrails;
    public float TrailLifetime;
    public float TrailSpawnDelta;
    public GameObject TrailPrefab;
}