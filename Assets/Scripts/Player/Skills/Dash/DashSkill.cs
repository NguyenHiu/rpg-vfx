
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashSkill : Skill
{
    protected new DashConfig Cfg;
    protected Vector2 m_dashDir;
    protected TrailsController m_trailCtrl;
    private float m_dashTimer;

    public DashSkill(DashConfig cfg, PlayerController player, SpriteRenderer sampleSr, Transform trailParent) : base(cfg, player)
    {
        Cfg = cfg;
        m_trailCtrl = new(player, sampleSr, trailParent, cfg);
    }

    public override void FixedUpdate(float dt, PlayerContext context)
    {
        base.FixedUpdate(dt, context);
        m_dashTimer -= dt;
        if (m_dashTimer <= 0)
        {
            Stop();
            return;
        }

        context.Direction = m_dashDir;
        context.Speed = Cfg.Speed;
        m_trailCtrl.FixedUpdate(dt);
    }

    public override void Use()
    {
        Debug.Log("DashSkill - Use");
        base.Use();
        Player.Anim.ChangeState(PState.DASH);
        m_timer = Cfg.Cooldown;
        m_dashTimer = Cfg.Duration;
        m_dashDir = Player.Rb.linearVelocity.normalized;
        m_trailCtrl.StartTrails();
    }

    public override void Stop()
    {
        Debug.Log("DashSkill - Stop");
        base.Stop();
        m_trailCtrl.EnoughTrails();

        var prvState = (PlayerState)Player.Anim.StateM.PreviousState;
        switch (prvState.Type)
        {
            case PState.WALK:
                Player.Anim.ChangeState(PState.IDLE);
                break;
            case PState.WALK_SIDE:
                Player.Anim.ChangeState(PState.IDLE_SIDE);
                break;
            case PState.WALK_BACK:
                Player.Anim.ChangeState(PState.IDLE_BACK);
                break;

            default:
                Player.Anim.ChangeState(PState.IDLE);
                break;
        }
    }
}

public class TrailsController
{
    public readonly PlayerController Player;
    private readonly SpriteRenderer m_sampleSr;
    public readonly Transform ParentTf;
    public readonly DashConfig Cfg;
    private List<GhostTrail> m_trailPool;
    private bool m_isShowing;
    private float m_timer;
    private int m_trailIdx;

    public TrailsController(PlayerController player, SpriteRenderer sample, Transform parentTf, DashConfig cfg)
    {
        Player = player;
        ParentTf = parentTf;
        Cfg = cfg;
        m_sampleSr = sample;
        InitTrails();
    }

    private void InitTrails()
    {
        m_trailPool = new();
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
            m_timer = Cfg.TrailSpawnDelta;
            SpawnTrail();
        }
    }

    private void SpawnTrail()
    {
        if (m_trailIdx >= m_trailPool.Count) m_trailIdx = 0;
        m_trailPool[m_trailIdx].StartTrail(m_sampleSr, Cfg.TrailLifetime);
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