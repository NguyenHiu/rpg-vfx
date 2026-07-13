using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponAnimController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] protected PlayerController m_player;
    public PlayerController Player => m_player;
    [SerializeField] protected WeaponController m_weapon;

    [Header("View Only")]
    [SerializeField] protected WeaponStateMachine m_stateM;
    [SerializeField] protected List<WeaponState> m_states;

    void Awake()
    {
        InitState();
    }

    protected virtual void InitState()
    {
        m_states = new()
        {
            new WeaponIdle(m_weapon, Player, "Idle")
        };

        m_stateM = new(GetState(WState.IDLE));
    }

    void Update()
    {
        m_stateM.CurrentState.Update();
    }

    public void ChangeState(WState state, Action callback = null)
    {
        m_stateM.ChangeState(GetState(state), callback);
    }

    private WeaponState GetState(WState state)
    {
        foreach (var s in m_states)
        {
            if (s.Type == state) return s;
        }

        return null;
    }
}
