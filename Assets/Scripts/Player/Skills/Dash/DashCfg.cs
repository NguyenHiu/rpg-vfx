using UnityEngine;

[CreateAssetMenu(fileName="DashConfig", menuName ="ScriptableObject/Skill Configs/Dash")]
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