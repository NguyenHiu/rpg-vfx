using DG.Tweening;

public class WeaponIdle : WeaponState
{
    public WeaponIdle(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.IDLE;
        DebugLog = true;
    }

    public override void Enter()
    {
        base.Enter();
        Weapon.transform.DOKill();

        var pos = Weapon.transform.localPosition;
        pos.y -= Weapon.YRange / 2f;
        Weapon.transform.localPosition = pos;

        Weapon.transform.DOLocalMoveY(pos.y + Weapon.YRange, Weapon.IdleSpeed / Player.SpeedBuff).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public override void Update()
    {
        base.Update();
        if (Player.AttackThisFrame) Weapon.ChangeState(WState.ATTACK);
    }
}