using System;
using UnityEngine;

public class MeleePierceAttackState : WeaponState
{
    public new MeleeController Weapon;
    private MeleePierceAttack m_skill;
    private float m_animSpeed;

    public MeleePierceAttackState(MeleeController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.PIERCE_ATTACK;
        Weapon = weapon;
    }

    public override void Enter()
    {
        base.Enter();
        m_animSpeed = Player.Anim.Anim.speed; // I know.. this looks ugly af
        Player.Anim.Anim.speed = 0f;
        m_skill ??= (MeleePierceAttack)Weapon.GetSkill(MELEE_SKILL.PIERCE_ATTACK);
        // TODO: Switch collider layer instead of turning on/off
        Player.MoveCollider.enabled = false;

        // Find position (set to the weapon)
        var attackPeak = Player.FacingDir * Weapon.BasicAttackCfg.Radius;
        var attackHandle = attackPeak.normalized * (attackPeak.magnitude - Weapon.WeaponLength);
        var attackPoint = attackHandle + Weapon.BasicAttackCfg.CenterOffset;
        Weapon.transform.localPosition = attackPoint;

        // Reset the SR
        Weapon.SR.transform.localPosition = Vector3.zero;

        // Rotote the sword
        var deltaAngle = Mathf.Atan2(Player.FacingDir.x, Player.FacingDir.y) * Mathf.Rad2Deg;
        Weapon.SR.transform.localEulerAngles = new(0, 0, -deltaAngle);

        m_skill?.Collider.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        Player.Anim.Anim.speed = m_animSpeed;
        Player.MoveCollider.enabled = true;
        m_skill?.Collider.gameObject.SetActive(false);
    }
}