
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashSkill : ActiveSkill
{
    protected new DashConfig Cfg;
    private Vector2 m_dashDir;
    private TrailsController m_trailCtrl;
    private float m_dashTimer;
    private PlayerState m_prvState;

    public DashSkill(DashConfig cfg, SkillController skillCtrl, InputAction action, SpriteRenderer sampleSr, Transform trailParent) : base(cfg, skillCtrl, action)
    {
        Cfg = cfg;
        m_trailCtrl = new(sampleSr, trailParent, cfg);
    }

    public override void FixedUpdate(float dt, PlayerContext context)
    {
        base.FixedUpdate(dt, context);
        m_dashTimer -= dt;
        if (m_dashTimer <= 0)
        {
            Cancel();
            return;
        }

        context.Direction = m_dashDir;
        context.Speed = Cfg.Speed;
        m_trailCtrl.FixedUpdate(dt);
    }

    public override bool Available()
    {
        return base.Available() && Player.Rb.linearVelocity != Vector2.zero;
    }

    public override void Activate()
    {
        base.Activate();
        m_prvState = Player.Anim.GetCurrentState();
        Player.Anim.ChangeState(PState.DASH);
        m_dashTimer = Cfg.Duration;
        m_dashDir = Player.Rb.linearVelocity.normalized;
        m_trailCtrl.StartTrails();
    }

    public override void Cancel()
    {
        base.Cancel();
        m_trailCtrl.EnoughTrails();

        switch (m_prvState.Type)
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
    private readonly SpriteRenderer m_sampleSr;
    public readonly Transform ParentTf;
    public readonly DashConfig Cfg;
    private List<GhostTrail> m_trailPool;
    private bool m_isShowing;
    private float m_timer;
    private int m_trailIdx;

    public TrailsController(SpriteRenderer sample, Transform parentTf, DashConfig cfg)
    {
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