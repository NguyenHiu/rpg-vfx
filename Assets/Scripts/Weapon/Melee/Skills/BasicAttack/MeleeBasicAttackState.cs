using System;
using UnityEngine;
using DG.Tweening;

public class MeleeBasicAttackState : WeaponAttack
{
    public new MeleeController Weapon;
    private MeleeBasicAttack m_skill;

    public MeleeBasicAttackState(MeleeController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Weapon = weapon;
    }

    public override void EnterCb(Action callback)
    {
        Debug.Log("[MeleeBasicAttackState] EnterCb");
        base.EnterCb(callback);

        m_skill ??= (MeleeBasicAttack) Weapon.GetSkill(MELEE_SKILL.BASIC_ATTACK);

        // Find position (set to the weapon)
        var attackPeak = Player.FacingDir * Weapon.BasicAttackCfg.Radius;
        var attackHandle = attackPeak.normalized * (attackPeak.magnitude - Weapon.WeaponLength);
        var attackPoint = attackHandle + Weapon.BasicAttackCfg.CenterOffset;
        Debug.Log($"attackPoint: {attackPoint}");
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
            m_skill?.Collider.gameObject.SetActive(false);
        });
        DOVirtual.DelayedCall(Weapon.BasicAttackCfg.Speed * 0.2f, () =>
        {
            if (m_skill != null) m_skill.Collider.transform.localEulerAngles = new(0, 0, -deltaAngle);
            m_skill?.Collider.gameObject.SetActive(true);
        });

        // Set the slash animation
        Weapon.SlashAnim.transform.localPosition = attackPeak;
        Weapon.SlashAnim.transform.localEulerAngles = new(0, 0, -deltaAngle);
        if (deltaAngle * Weapon.SlashAnim.transform.localScale.x < 0)
        {
            var localScale = Weapon.SlashAnim.transform.localScale;
            localScale.x *= -1;
            Weapon.SlashAnim.transform.localScale = localScale;
        }
        Weapon.SlashAnim.SetTrigger("Slash");
    }


    public override void Exit()
    {
        base.Exit();
        Weapon.SR.transform.DOKill();
    }
}