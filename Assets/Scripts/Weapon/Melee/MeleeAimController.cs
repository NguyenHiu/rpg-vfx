using UnityEngine;

public class MeleeAimController : WeaponAnimController
{
    private MeleeController m_meleeWeapon;
    
    protected override void InitState()
    {
        base.InitState();
        m_meleeWeapon = (MeleeController) m_weapon;
        m_states.Add(new MeleeBasicAttackState(m_meleeWeapon, m_player, "Attack"));
        m_states.Add(new MeleePierceAttackState(m_meleeWeapon, m_player, "Pierce Attack"));
        m_states.Add(new MeleeSlashComboState(m_meleeWeapon, m_player, "Slash Combo Attack"));
    }
}