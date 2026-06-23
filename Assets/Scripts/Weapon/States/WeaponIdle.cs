using DG.Tweening;

public class WeaponIdle : WeaponState
{
    public float OriginY;

    public WeaponIdle(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.IDLE;
        DebugLog = true;
    }

    public override void Enter()
    {
        base.Enter();
        Weapon.transform.DOKill();

        OriginY = Weapon.transform.localPosition.y;
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

    public override void Exit()
    {
        base.Exit();
        Weapon.transform.localPosition = new(Weapon.transform.localPosition.x, OriginY);
    }
}