using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    public PlayerController Player;
    public StateMachine StateM;
    public List<WeaponState> States;

    [Header("===== Stats =====")]
    [Header("Idle")]
    public float YRange;
    public float IdleSpeed;

    [Header("Attack")]
    public float AttackSpeed;
    public Vector2 AngleRange;

    void Awake()
    {
        States = new()
        {
            new WeaponIdle(this, Player, "Idle"),
            new WeaponAttack(this, Player, "Attack"),
        };


        StateM = new(GetState(WState.IDLE));
    }

    void Update()
    {
        StateM.CurrentState.Update();
    }

    public void ChangeState(WState state)
    {
        StateM.ChangeState(GetState(state));
    }

    private WeaponState GetState(WState state)
    {
        foreach (var s in States)
        {
            if (s.Type == state) return s;
        }

        return null;
    }


}
