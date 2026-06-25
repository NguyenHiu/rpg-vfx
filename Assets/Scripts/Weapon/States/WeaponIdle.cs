using DG.Tweening;
using UnityEngine;

public class WeaponIdle : WeaponState
{
    public Vector2 OriginalPos;
    public Vector2 IdlePos;

    public WeaponIdle(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.IDLE;
        DebugLog = true;

        // Used to restore original position
        OriginalPos = Weapon.SR.transform.localPosition;

        // Pos to start idling (bottom)
        IdlePos = new(OriginalPos.x, OriginalPos.y - Weapon.YRange / 2f);
    }

    public override void Enter()
    {
        base.Enter();
        Weapon.SR.transform.DOKill();

        // Set the sprite to the bottom position
        Weapon.SR.transform.localPosition = IdlePos;
        Weapon.SR.transform.localEulerAngles = new(0, 0, Weapon.IdleAngle);

        // Start moving
        Weapon.SR.transform.DOLocalMoveY(IdlePos.y + Weapon.YRange, Weapon.IdleSpeed / Player.SpeedBuff).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public override void Update()
    {
        base.Update();

        // Switch to attack state
        if (Player.AttackThisFrame)
            Weapon.ChangeState(WState.ATTACK);
    }

    public override void Exit()
    {
        base.Exit();

        // Restore
        Weapon.SR.transform.localPosition = OriginalPos;
        Weapon.SR.transform.localEulerAngles = Vector3.zero;
    }
}