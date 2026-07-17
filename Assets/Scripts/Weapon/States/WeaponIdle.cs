using DG.Tweening;
using UnityEngine;

public class WeaponIdle : WeaponState
{
    private Vector3 m_idlePos, m_idleAngle;

    public WeaponIdle(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.IDLE;
        DebugLog = false;
    }

    public override void Enter()
    {
        base.Enter();

        // Pos to start idling (bottom)
        m_idlePos = new(Weapon.IdleAnimCfg.OffsetRight.x, Weapon.IdleAnimCfg.OffsetRight.y - Weapon.IdleAnimCfg.YRange / 2f);
        m_idleAngle = new(0, 0, Weapon.IdleAnimCfg.Angle);

        // Set the sprite to the bottom position
        if (Player.FacingDir.x * m_idlePos.x < 0)
        {
            m_idlePos.x *= -1;
            m_idleAngle.z *= -1;
        }
        Weapon.transform.localPosition = Vector2.zero;
        Weapon.SR.transform.localPosition = m_idlePos;
        Weapon.SR.transform.localEulerAngles = m_idleAngle;

        // Start moving
        Weapon.SR.transform.DOLocalMoveY(m_idlePos.y + Weapon.IdleAnimCfg.YRange, Weapon.IdleAnimCfg.IdleSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public override void Update()
    {
        base.Update();

        // Update position & rotate when switching side
        if (Player.FacingDir.x * m_idlePos.x < 0)
        {
            m_idlePos.x *= -1;
            m_idleAngle.z *= -1;
            Weapon.SR.transform.localPosition = m_idlePos;
            Weapon.SR.transform.localEulerAngles = m_idleAngle;
        }
    }

    public override void Exit()
    {
        base.Exit();
        Weapon.SR.transform.DOKill();
    }
}