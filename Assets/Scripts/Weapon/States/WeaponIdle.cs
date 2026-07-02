using DG.Tweening;
using UnityEngine;

public class WeaponIdle : WeaponState
{
    private Vector3 m_idlePos, m_idleAngle;
    private Vector3 m_SRLocalPosition, m_SRLocalEulerAngles;

    public WeaponIdle(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.IDLE;
        DebugLog = false;

        // Pos to start idling (bottom)
        m_idlePos = new(Weapon.IdlePointRight.localPosition.x, Weapon.IdlePointRight.localPosition.y - Weapon.YRange / 2f);
        m_idleAngle = new(0, 0, Weapon.IdleAngle);
    }

    public override void Enter()
    {
        base.Enter();
        // Restore when existing
        m_SRLocalPosition = Weapon.SR.transform.localPosition;
        m_SRLocalEulerAngles = Weapon.SR.transform.localEulerAngles;

        // Set the sprite to the bottom position
        if (Player.FacingDir.x * m_idlePos.x < 0)
        {
            m_idlePos.x *= -1;
            m_idleAngle.z *= -1;
        }
        Weapon.SR.transform.localPosition = m_idlePos;
        Weapon.SR.transform.localEulerAngles = m_idleAngle;

        // Start moving
        Weapon.SR.transform.DOLocalMoveY(m_idlePos.y + Weapon.YRange, Weapon.IdleSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public override void Update()
    {
        base.Update();

        // Switch to attack state
        if (Player.IsAbleToAttack())
            Weapon.ChangeState(WState.ATTACK);

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

        // Restore
        Weapon.SR.transform.DOKill();
        Weapon.SR.transform.localPosition = m_SRLocalPosition;
        Weapon.SR.transform.localEulerAngles = m_SRLocalEulerAngles;
    }
}