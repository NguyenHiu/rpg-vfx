using UnityEngine;

public class MeleeWeaponAimController : WeaponAnimController
{
    protected new MeleeWeaponController m_weapon;

    protected override void InitState()
    {
        base.InitState();
        m_states.Add(new WeaponMeleeBasicAttack(m_weapon, m_player, "attack"));
    }
}