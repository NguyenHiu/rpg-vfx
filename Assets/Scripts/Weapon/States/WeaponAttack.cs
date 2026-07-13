using System;
using DG.Tweening;

public class WeaponAttack : WeaponState
{
    public WeaponAttack(WeaponController weapon, PlayerController player, string name) : base(weapon, player, name)
    {
        Type = WState.ATTACK;
        DebugLog = false;
    }
}