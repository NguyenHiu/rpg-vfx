using System;
using DG.Tweening;

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

        DOVirtual.DelayedCall(.5f, () =>
        {
            Callback?.Invoke();
        });
    }
}