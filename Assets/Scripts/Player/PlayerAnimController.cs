using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public const float VIRTUAL_DIR_RANGE = 0.58f;
    [field: SerializeField] public PlayerController Player {get; private set; }
    [field: SerializeField] public Animator Anim {get; private set; }
    [field: SerializeField] public StateMachine StateM {get; private set; }
    private List<PlayerState> m_states;

    void Awake()
    {
        m_states = new()
        {
            new PlayerIdle(Player, this, "Idle"),
            new PlayerIdleSide(Player, this, "Idle_Side"),
            new PlayerIdleBack(Player, this, "Idle_Back"),
            new PlayerWalk(Player, this, "Walk"),
            new PlayerWalkSide(Player, this, "Walk_Side"),
            new PlayerWalkBack(Player, this, "Walk_Back"),
            new PlayerDash(Player, this),
        };

        StateM = new(GetState(PState.IDLE));

        // Init default value
        OnSpeedBuffChange(Player.SpeedBuff);

        // Register for future changes
        Player.OnSpeedBuffChange += OnSpeedBuffChange;
    }

    void Update()
    {
        StateM.CurrentState.Update();
    }

    public void ChangeState(PState type)
    {
        StateM.ChangeState(GetState(type));
    }

    PlayerState GetState(PState type)
    {
        for (int i = 0; i < m_states.Count; i++)
        {
            if (m_states[i].Type == type) return m_states[i];
        }
        return null;
    }

    private void OnSpeedBuffChange(float val)
    {
        Anim.speed = val;
    }
}
