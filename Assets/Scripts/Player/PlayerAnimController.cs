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
    DASH,
}

public class PlayerAnimController : MonoBehaviour
{
    public const float VIRTUAL_DIR_RANGE = 0.58f;
    public PlayerController Player;
    public Animator Anim;
    public StateMachine StateM;
    public List<PlayerState> States;

    void Awake()
    {
        States = new()
        {
            new PlayerIdle(Player, this, "Idle"),
            new PlayerIdleSide(Player, this, "Idle_Side"),
            new PlayerIdleBack(Player, this, "Idle_Back"),
            new PlayerWalk(Player, this, "Walk"),
            new PlayerWalkSide(Player, this, "Walk_Side"),
            new PlayerWalkBack(Player, this, "Walk_Back"),
            new PlayerDash(Player, this),
        };

        StateM = new(GetState(PlayerStates.IDLE));

        // Init default value
        OnSpeedBuffChange(Player.SpeedBuff);

        // Register for future changes
        Player.OnSpeedBuffChange += OnSpeedBuffChange;
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
            if (States[i].Type == type) return States[i];
        }
        return null;
    }

    private void OnSpeedBuffChange(float val)
    {
        Anim.speed = val;
    }
}
