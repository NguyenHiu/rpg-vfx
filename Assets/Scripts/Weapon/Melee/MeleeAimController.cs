using UnityEngine;

public class MeleeAimController : WeaponAnimController
{
    protected new MeleeController m_weapon;

    protected override void InitState()
    {
        base.InitState();
        m_states.Add(new MeleeBasicAttackState(m_weapon, m_player, "attack"));
    }
}