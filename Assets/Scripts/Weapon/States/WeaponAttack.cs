using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponAttack : WeaponState
{
    public Vector2 OriginLocalPos;
    public Vector3 OriginLocalEulerAngles;

    public WeaponAttack(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.ATTACK;
        DebugLog = true;
    }

    public override void Enter()
    {
        base.Enter();
        Player.IsAttacking = true;
        Weapon.transform.DOKill();
        OriginLocalPos = Weapon.transform.localPosition;
        OriginLocalEulerAngles = Weapon.transform.localEulerAngles;

        Player.SlashAnim.SetBool("IsAttacking", true);
        Weapon.transform.localEulerAngles = new(0, 0, Weapon.AngleRange.x);
        Weapon.transform.DOLocalRotate(new(0, 0, Weapon.AngleRange.y), Weapon.AttackSpeed).OnComplete(() =>
        {
            Weapon.ChangeState(WState.IDLE);
        });
    }

    public override void Exit()
    {
        base.Exit();
        Weapon.transform.localPosition = OriginLocalPos;
        Weapon.transform.localEulerAngles = OriginLocalEulerAngles;
        Player.IsAttacking = false;
        Player.SlashAnim.SetBool("IsAttacking", false);
    }
}