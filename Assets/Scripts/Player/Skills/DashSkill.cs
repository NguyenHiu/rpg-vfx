
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{
    protected new DashCfg Cfg;
    protected Vector2 m_dashDir;
    protected TrailsController m_trailCtrl;
    private float m_dashTimer;

    public DashSkill(DashCfg cfg, PlayerController player, SpriteRenderer body, Transform trailParent) : base(cfg, player)
    {
        Cfg = cfg;
        m_trailCtrl = new(player, body, trailParent, cfg);
    }

    public Vector2 GetDir() => m_dashDir;

    public override void FixedUpdate(float dt)
    {
        base.FixedUpdate(dt);
        m_dashTimer -= dt;
        if (m_dashTimer <= 0)
            Exit();
        m_trailCtrl.FixedUpdate(dt);
    }

    public override void Enter()
    {
        Debug.Log("DashSkill - Enter");
        base.Enter();
        m_timer = Cfg.Cooldown;
        m_dashTimer = Cfg.Duration;
        m_dashDir = Player.Rb.linearVelocity.normalized;
        m_trailCtrl.StartTrails();
    }

    public override void Exit()
    {
        Debug.Log("DashSkill - Exit");
        base.Exit();
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

    public float GetSpeed()
    {
        Debug.Log("DashSkill - GetSpeed: " + Cfg.Speed);
        return Cfg.Speed;
    }
}

public class TrailsController
{
    public readonly PlayerController Player;
    private readonly SpriteRenderer m_sr;
    public readonly Transform ParentTf;
    public readonly DashCfg Cfg;
    private List<GhostTrail> m_trailPool;
    private bool m_isShowing;
    private float m_timer;
    private int m_trailIdx;

    public TrailsController(PlayerController player, SpriteRenderer body, Transform parentTf, DashCfg cfg)
    {
        Player = player;
        ParentTf = parentTf;
        Cfg = cfg;
        m_sr = body;
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