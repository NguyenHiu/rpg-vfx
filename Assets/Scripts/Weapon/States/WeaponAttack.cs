using DG.Tweening;
using UnityEngine;

public class WeaponAttack : WeaponState
{
    public Vector2 OriginalPos;

    public WeaponAttack(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.ATTACK;
        DebugLog = true;
    }

    public override void Enter()
    {
        base.Enter();
        Player.IsAttacking = true;
        Weapon.SR.transform.DOKill();
        OriginalPos = Weapon.SR.transform.localPosition;

        // TODO: change position
        // 1. Calculate 
        var attackPeak = Player.FacingDir * Weapon.AttackRadius;
        var attackHandle = attackPeak.normalized * (attackPeak.magnitude - Weapon.Head.transform.localPosition.magnitude);
        var attackPoint = attackHandle + (Vector2)Weapon.AttackCenter.transform.localPosition;
        Debug.Log($"peak: {attackPeak}; handle: {attackHandle}; point: {attackPoint}");
        Weapon.transform.localPosition = attackPoint;

        Weapon.SlashAnim.SetBool("IsAttacking", true);
        Weapon.SR.transform.localEulerAngles = new(0, 0, Weapon.AngleRange.x);
        Weapon.SR.transform.DOLocalRotate(new(0, 0, Weapon.AngleRange.y), Weapon.AttackSpeed).OnComplete(() =>
        {
            Weapon.ChangeState(WState.IDLE);
        });
    }

    public override void Exit()
    {
        base.Exit();
        Weapon.SR.transform.localPosition = OriginalPos;
        Weapon.SR.transform.localEulerAngles = Vector3.zero;

        Player.IsAttacking = false;
        Weapon.SlashAnim.SetBool("IsAttacking", false);
    }
}