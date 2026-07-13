
using System;

public enum WState
{
    IDLE,
    ATTACK,
}

public class WeaponState : State
{
    public WState Type { get; protected set; }
    public WeaponController Weapon;
    public PlayerController Player;
    public Action Callback;

    public WeaponState(WeaponController weapon, PlayerController player, string name) : base(name)
    {
        Weapon = weapon;
        Player = player;
    }

    public virtual void EnterCb(Action callback)
    {
        Callback = callback;
        base.Enter();
    }
}