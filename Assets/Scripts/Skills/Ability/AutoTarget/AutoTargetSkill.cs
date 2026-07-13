using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class AutoTargetSkill : PassiveSkill
{
    public List<GameObject> Targets;
    protected new AutoTargetCfg Cfg;
    private readonly Transform m_centerTf;
    private readonly List<Collider2D> m_buffer = new(32);
    private readonly HashSet<Transform> m_seen = new();

    public AutoTargetSkill(AutoTargetCfg cfg, SkillController skilLCtrl, Transform centerTf) : base(cfg, skilLCtrl)
    {
        Cfg = cfg;
        m_centerTf = centerTf;
        Targets = new();
    }

    public override bool MeetCondition()
    {
        // ALWAYS AUTO TARGET IN ATTACK MODE
        return Available() && Player.Mode == PlayerMode.ATTACK;
    }

    public override void FixedUpdate(float dt, PlayerContext context)
    {
        if (!IsRunning) return;
        
        base.FixedUpdate(dt, context);
        if (!IsEnable) return;
        Debug.Log("[AutoTargetSkill] - Fixed Update");
        context.Targets = Targets;
    }

    public override void Activate()
    {
        base.Activate();

        // Find nearest enemy in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(m_centerTf.position, Cfg.Radius, Cfg.EnemyLayer);
        int count = FilterAndSort(hits);
        int take = Math.Min(Cfg.NTarget, count);
        Targets.Clear();
        for (int i = 0; i < take; i++)
            Targets.Add(m_buffer[i].gameObject);
    }

    private int FilterAndSort(Collider2D[] hits)
    {
        m_buffer.Clear();
        m_seen.Clear();

        foreach (var c in hits)
        {
            var root = c.transform.root;
            if (m_seen.Add(root))
                m_buffer.Add(c);
        }

        Vector3 center = m_centerTf.position;
        m_buffer.Sort((a, b) =>
        {
            float da = (a.transform.position - center).sqrMagnitude;
            float db = (b.transform.position - center).sqrMagnitude;
            return da.CompareTo(db);
        });

        return m_buffer.Count;
    }


#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        float angleStep = 360f / 10;
        Vector3 prevPoint = m_centerTf.position + new Vector3(Cfg.Radius, 0f, 0f);

        for (int i = 1; i <= 10; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPoint = m_centerTf.position + new Vector3(Mathf.Cos(angle) * Cfg.Radius, Mathf.Sin(angle) * Cfg.Radius, 0f);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
#endif
}