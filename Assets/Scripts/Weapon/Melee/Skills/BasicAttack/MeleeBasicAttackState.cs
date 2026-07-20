using System;
using UnityEngine;
using DG.Tweening;

public class MeleeBasicAttackState : WeaponState
{
    public new MeleeController Weapon;
    private MeleeBasicAttack m_skill;

    public MeleeBasicAttackState(MeleeController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.ATTACK;
        Weapon = weapon;
    }

    public override void EnterCb(Action callback)
    {
        base.EnterCb(callback);

        m_skill ??= (MeleeBasicAttack)Weapon.GetSkill(MELEE_SKILL.BASIC_ATTACK);

        // Find position (set to the weapon)
        var attackPeak = Player.FacingDir * Weapon.BasicAttackCfg.Radius;
        var attackHandle = attackPeak.normalized * (attackPeak.magnitude - Weapon.WeaponLength);
        var attackPoint = attackHandle + Weapon.BasicAttackCfg.CenterOffset;
        Weapon.transform.localPosition = attackPoint;

        // Reset the SR
        Weapon.SR.transform.localPosition = Vector3.zero;

        /// Find rotation (set to the sprite only)
        // Rotote the sword
        var dir = Player.FacingDir.x >= 0 ? -1 : 1;
        var deltaAngle = Mathf.Atan2(Player.FacingDir.x, Player.FacingDir.y) * Mathf.Rad2Deg;
        Weapon.SR.transform.localEulerAngles = new(0, 0, -Weapon.BasicAttackCfg.Angle * dir - deltaAngle);

        // Set the slash animation
        Weapon.SlashAnim.transform.localPosition = attackPeak;
        Weapon.SlashAnim.transform.localEulerAngles = new(0, 0, -deltaAngle);
        if (deltaAngle * Weapon.SlashAnim.transform.localScale.x > 0)
        {
            var localScale = Weapon.SlashAnim.transform.localScale;
            localScale.x *= -1;
            Weapon.SlashAnim.transform.localScale = localScale;
        }

        Weapon.SR.transform
            .DOLocalRotate(new(0, 0, Weapon.BasicAttackCfg.Angle * dir - deltaAngle), Weapon.BasicAttackCfg.Speed)
            .OnComplete(() =>
            {
                Callback?.Invoke();
                m_skill?.Collider.gameObject.SetActive(false);
            })
            .SetEase(Ease.OutCubic);

        // Enable Collider -> Hit
        DOVirtual.DelayedCall(Weapon.BasicAttackCfg.Speed * 0.2f, () =>
        {
            if (m_skill != null) m_skill.Collider.transform.localEulerAngles = new(0, 0, -deltaAngle);
            m_skill?.Collider.gameObject.SetActive(true);
        });

        // Show slash sprite
        DOVirtual.DelayedCall(Weapon.BasicAttackCfg.Speed * 0.4f, () =>
        {
            Weapon.SlashAnim.GetComponent<SpriteRenderer>().enabled = true;
        });

        // Hide slash sprite
        DOVirtual.DelayedCall(Weapon.BasicAttackCfg.Speed * 0.7f, () =>
        {
            Weapon.SlashAnim.GetComponent<SpriteRenderer>().enabled = false;
        });
    }


    public override void Exit()
    {
        base.Exit();
        Weapon.SR.transform.DOKill();
    }
}