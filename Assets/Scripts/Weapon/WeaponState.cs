
using UnityEngine;

public struct LocalTransformSnapshot
{
    private Vector3 m_localPosition;
    private Vector3 m_localEuler;
    private Vector3 m_localScale;

    public LocalTransformSnapshot(Transform t)
    {
        m_localPosition = t.localPosition;
        m_localEuler = t.localEulerAngles;
        m_localScale = t.localScale;
    }

    public void RestoreTo(Transform t)
    {
        t.localPosition = m_localPosition;
        t.localEulerAngles = m_localEuler;
        t.localScale = m_localScale;
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