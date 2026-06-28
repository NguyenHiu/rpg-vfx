using DG.Tweening;
using UnityEngine;

public class WeaponAttack : WeaponState
{
    public Vector3 LocalPosition, SRLocalPosition, SRLocalScale, SRLocalEulerAngle;
    public WeaponAttack(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.ATTACK;
        DebugLog = false;
    }

    public override void Enter()
    {
        base.Enter();
        // Restore when existing
        LocalPosition = Weapon.transform.localPosition;
        SRLocalPosition = Weapon.SR.transform.localPosition;
        SRLocalScale = Weapon.SR.transform.localScale;
        SRLocalEulerAngle = Weapon.SR.transform.localEulerAngles;
        
        Player.IsAttacking = true;
        Weapon.PC2D.gameObject.SetActive(true);

        /// Find position (set to the weapon)
        var attackPeak = Player.FacingDir * Weapon.AttackRadius;
        var attackHandle = attackPeak.normalized * (attackPeak.magnitude - Weapon.WeaponLength);
        var attackPoint = attackHandle + (Vector2)Weapon.AttackCenter.transform.localPosition;
        Weapon.transform.localPosition = attackPoint;

        // Reset the SR
        Weapon.SR.transform.localPosition = Vector3.zero;

        /// Find rotation (set to the sprite only)
        // Correct the slam direction
        var dir = Player.FacingDir.x >= 0 ? -1 : 1;
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
        Weapon.PC2D.transform.localEulerAngles = new(0, 0, -deltaAngle); // Do not need to restore this one

        // Change the anim to attack
        Weapon.Anim.SetTrigger("Attack");
    }

    public override void Exit()
    {
        base.Exit();

        // Restore
        Weapon.transform.localPosition = LocalPosition;
        Weapon.SR.transform.localPosition = SRLocalPosition;
        Weapon.SR.transform.localScale = SRLocalScale;
        Weapon.SR.transform.localEulerAngles = SRLocalEulerAngle;
        
        Player.IsAttacking = false;
        Weapon.PC2D.gameObject.SetActive(false);
        Weapon.SR.transform.DOKill();
        Player.ResetAttackTimer();
    }
}