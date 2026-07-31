using System;
using DG.Tweening;
using UnityEngine;

public class MeleeSlashComboState : WeaponState
{
    public new MeleeController Weapon;
    private MeleeSlashCombo m_skill;

    public MeleeSlashComboState(MeleeController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.SLASH_COMBO;
        Weapon = weapon;
    }

    public override void EnterCb(Action callback)
    {
        base.EnterCb(callback);

        // Get skill
        m_skill ??= (MeleeSlashCombo)Weapon.GetSkill(MELEE_SKILL.SLASH_COMBO);
        if (m_skill == null)
        {
            Debug.LogError("[MeleeSlashComboState] Missing Combo Slash Skill");
            return;
        }

        // TODO:
        // Take current idx
        // Perform attack at current idx

        var step = m_skill.StepIdx;
        if (step == 0) Attack1();
        else if (step == 1) Attack2();
        else Attack3();
    }

    private void Attack1()
    {
        Debug.Log("Attack 1");
        DOVirtual.DelayedCall(.5f, () =>
        {
            Callback?.Invoke();
        });
    }

    private void Attack2()
    {
        Debug.Log("Attack 2");
        DOVirtual.DelayedCall(.5f, () =>
        {
            Callback?.Invoke();
        });
    }

    private void Attack3()
    {
        Debug.Log("Attack 3");
        DOVirtual.DelayedCall(.5f, () =>
        {
            Callback?.Invoke();
        });
    }
}