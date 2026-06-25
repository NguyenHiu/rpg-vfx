
using UnityEngine;

public struct LocalTransformSnapshot
{
    public Vector3 LocalPosition;
    public Vector3 LocalEuler;
    public Vector3 LocalScale;

    public LocalTransformSnapshot(Transform t)
    {
        LocalPosition = t.localPosition;
        LocalEuler = t.localEulerAngles;
        LocalScale = t.localScale;
    }

    public void RestoreTo(Transform t)
    {
        t.localPosition = LocalPosition;
        t.localEulerAngles = LocalEuler;
        t.localScale = LocalScale;
    }
}

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