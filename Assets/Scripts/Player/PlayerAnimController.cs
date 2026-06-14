using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    IDLE,
    IDLE_SIDE,
    IDLE_BACK,
    WALK,
    WALK_SIDE,
    WALK_BACK,
}

[System.Serializable]
public struct PlayerStateInfo
{
    public PlayerStates StateType;
    public PlayerState State;

    public PlayerStateInfo(PlayerStates type, PlayerState state)
    {
        StateType = type;
        State = state;
    }
}

public class PlayerAnimController : MonoBehaviour
{
    public const float VIRTUAL_DIR_RANGE = 0.58f;
    public PlayerController Player;
    public Animator Anim;
    public StateMachine StateM;
    public List<PlayerStateInfo> States;

    void Awake()
    {
        States = new()
        {
            new(PlayerStates.IDLE, new PlayerIdle(Player, this, "Idle")),
            new(PlayerStates.IDLE_SIDE, new PlayerIdleSide(Player, this, "Idle_Side")),
            new(PlayerStates.IDLE_BACK, new PlayerIdleBack(Player, this, "Idle_Back")),
            new(PlayerStates.WALK, new PlayerWalk(Player, this, "Walk")),
            new(PlayerStates.WALK_SIDE, new PlayerWalkSide(Player, this, "Walk_Side")),
            new(PlayerStates.WALK_BACK, new PlayerWalkBack(Player, this, "Walk_Back")),
        };

        StateM = new(GetState(PlayerStates.IDLE));
    }

    void Update()
    {
        StateM.CurrentState.Update();
    }

    public void ChangeState(PlayerStates type)
    {
        StateM.ChangeState(GetState(type));
    }

    PlayerState GetState(PlayerStates type)
    {
        for (int i = 0; i < States.Count; i++)
        {
            if (States[i].StateType == type) return States[i].State;
        }
        return null;
    }
}
