
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

    public WeaponState(WeaponController weapon, PlayerController player, string name) : base(name)
    {
        Weapon = weapon;
        Player = player;
    }
}