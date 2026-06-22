using DG.Tweening;

public class WeaponAttack : WeaponState
{
    public Sequence sequence;

    public WeaponAttack(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.ATTACK;
        DebugLog = true;
    }

    public override void Enter()
    {
        base.Enter();
        Weapon.transform.DOKill();
        Weapon.transform.localEulerAngles = new(0, 0, Weapon.AngleRange.x);

        // sequence = DOTween.Sequence();
        // sequence.Append(
        //     Weapon.transform.DORotate(new(0, 0, Weapon.AngleRange.y), Weapon.AttackSpeed)
        // )
        Weapon.transform.DOLocalRotate(new(0, 0, Weapon.AngleRange.y), Weapon.AttackSpeed).OnComplete(() =>
        {
            Weapon.ChangeState(WState.IDLE);
        });
    }

    public override void Exit()
    {
        base.Exit();
        sequence?.Kill();
    }
}