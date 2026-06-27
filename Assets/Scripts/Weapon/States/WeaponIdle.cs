using DG.Tweening;
using UnityEngine;

public class WeaponIdle : WeaponState
{
    public Vector3 IdlePos, IdleAngle;
    public Vector3 SRLocalPosition, SRLocalEulerAngles;

    public WeaponIdle(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.IDLE;
        DebugLog = true;

        // Pos to start idling (bottom)
        IdlePos = new(Weapon.IdlePointRight.localPosition.x, Weapon.IdlePointRight.localPosition.y - Weapon.YRange / 2f);
        IdleAngle = new(0, 0, Weapon.IdleAngle);
    }

    public override void Enter()
    {
        base.Enter();
        // Restore when existing
        SRLocalPosition = Weapon.SR.transform.localPosition;
        SRLocalEulerAngles = Weapon.SR.transform.localEulerAngles;

        // Set the sprite to the bottom position
        if (Player.FacingDir.x * IdlePos.x < 0)
        {
            IdlePos.x *= -1;
            IdleAngle.z *= -1;
        }
        Weapon.SR.transform.localPosition = IdlePos;
        Weapon.SR.transform.localEulerAngles = IdleAngle;

        // Start moving
        Weapon.SR.transform.DOLocalMoveY(IdlePos.y + Weapon.YRange, Weapon.IdleSpeed / Player.SpeedBuff).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public override void Update()
    {
        base.Update();

        // Switch to attack state
        if (Player.IsAbleToAttack())
            Weapon.ChangeState(WState.ATTACK);

        // Update position & rotate when switching side
        if (Player.FacingDir.x * IdlePos.x < 0)
        {
            IdlePos.x *= -1;
            IdleAngle.z *= -1;
            Weapon.SR.transform.localPosition = IdlePos;
            Weapon.SR.transform.localEulerAngles = IdleAngle;
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Restore
        Weapon.SR.transform.DOKill();
        Weapon.SR.transform.localPosition = SRLocalPosition;
        Weapon.SR.transform.localEulerAngles = SRLocalEulerAngles;
    }
}