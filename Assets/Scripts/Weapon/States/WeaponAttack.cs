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

        /// Find position (set to the weapon)
        var attackPeak = Player.FacingDir * Weapon.AttackRadius;
        var attackHandle = attackPeak.normalized * (attackPeak.magnitude - Weapon.WeaponLength);
        var attackPoint = attackHandle + (Vector2)Weapon.AttackCenter.transform.localPosition;
        Weapon.transform.localPosition = attackPoint;

        /// Find rotation (set to the sprite only)
        // Correct the slam direction
        var dir = Player.FacingDir.x > 0 ? -1 : 1;
        var scale = Weapon.SR.transform.localScale;
        if (Player.FacingDir.x * scale.x > 0) scale.x *= -1;
        Weapon.SR.transform.localScale = scale;

        // Rotote the sword
        var deltaAngle = Mathf.Atan2(Player.FacingDir.x, Player.FacingDir.y) * Mathf.Rad2Deg;
        Weapon.SR.transform.localEulerAngles = new(0, 0, -Weapon.AttackAngle * dir - deltaAngle);
        Weapon.SR.transform.DOLocalRotate(new(0, 0, Weapon.AttackAngle * dir - deltaAngle), Weapon.AttackSpeed).OnComplete(() =>
        {
            Weapon.ChangeState(WState.IDLE);
        });
        Weapon.PC2D.transform.localEulerAngles = new(0, 0, -deltaAngle);

        // Change the anim to attack
        Weapon.Anim.SetTrigger("Attack");
    }

    public override void Exit()
    {
        base.Exit();

        Player.IsAttacking = false;
        Player.ResetAttackTimer();
        Weapon.SR.transform.localPosition = OriginalPos;
    }
}