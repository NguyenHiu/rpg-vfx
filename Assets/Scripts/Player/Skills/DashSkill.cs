
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{
    protected Vector2 m_dashDir;
    protected TrailsController m_trailCtrl;

    public DashSkill(DashCfg cfg, PlayerController player, Transform trailParent) : base(cfg, player)
    {
        m_trailCtrl = new(player, trailParent, cfg);
    }

    public Vector2 GetDir() => m_dashDir;

    public override void FixedUpdate(float dt)
    {
        base.FixedUpdate(dt);
        m_trailCtrl.FixedUpdate(dt);
    }

    public override void Enter()
    {
        base.Enter();
        m_dashDir = Player.Rb.linearVelocity.normalized;
    }
}

public class TrailsController
{
    public readonly PlayerController Player;
    private SpriteRenderer m_sr;
    public readonly Transform ParentTf;
    public readonly DashCfg Cfg;
    private List<GhostTrail> m_trailPool;
    private bool m_isShowing;
    private float m_timer;
    private int m_trailIdx;

    public TrailsController(PlayerController player, Transform parentTf, DashCfg cfg)
    {
        Player = player;
        ParentTf = parentTf;
        Cfg = cfg;

        if (Player.TryGetComponent<SpriteRenderer>(out var sr))
        {
            m_sr = sr;
        }
        else
        {
            Debug.LogError("[DashSkill] - Player missing SpriteRenderer");
        }
        InitTrails();
    }

    private void InitTrails()
    {
        for (var i = 0; i < Cfg.NGhostTrails; i++)
        {
            var obj = Object.Instantiate(Cfg.TrailPrefab, ParentTf);
            m_trailPool.Add(obj.GetComponent<GhostTrail>());
            obj.SetActive(false);
        }
        m_isShowing = false;
        m_trailIdx = 0;
    }

    public void FixedUpdate(float dt)
    {
        if (!m_isShowing) return;

        m_timer -= dt;
        if (m_timer < 0)
        {
            m_timer = Cfg.TrailSpawnDelta / Player.SpeedBuff;
            SpawnTrail();
        }
    }

    private void SpawnTrail()
    {
        if (m_trailIdx >= m_trailPool.Count) m_trailIdx = 0;
        m_trailPool[m_trailIdx].StartTrail(m_sr, Cfg.TrailLifetime);
        m_trailIdx++;
    }

    public void StartTrails()
    {
        m_isShowing = true;
        m_timer = 0;
    }

    public void EnoughTrails()
    {
        m_isShowing = false;
        // Spawn one more trail
        SpawnTrail();
    }
}