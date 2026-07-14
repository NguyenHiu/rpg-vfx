using System;
using UnityEngine;
using DG.Tweening;

public class MeleeBasicAttackState : WeaponAttack
{
    public new MeleeController Weapon;

    public MeleeBasicAttackState(MeleeController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Weapon = weapon;
    }

    public override void EnterCb(Action callback)
    {
        base.EnterCb(callback);

        // TODO: Enable m_basicAttackCollider

        // Find position (set to the weapon)
        var attackPeak = Player.FacingDir * Weapon.BasicAttackCfg.Radius;
        var attackHandle = attackPeak.normalized * (attackPeak.magnitude - Weapon.WeaponLength);
        var attackPoint = attackHandle + Weapon.BasicAttackCfg.CenterOffset;
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
        Weapon.SR.transform.localEulerAngles = new(0, 0, -Weapon.BasicAttackCfg.Angle * dir - deltaAngle);
        Weapon.SR.transform.DOLocalRotate(new(0, 0, Weapon.BasicAttackCfg.Angle * dir - deltaAngle), Weapon.BasicAttackCfg.Speed).OnComplete(() =>
        {
            Callback?.Invoke();
        });

        // TODO: rotate m_basicAttackCollider instead
        // Weapon.PC2D.transform.localEulerAngles = new(0, 0, -deltaAngle); // Do not need to restore this one
    }


    public override void Exit()
    {
        base.Exit();
        Weapon.SR.transform.DOKill();
    }
}