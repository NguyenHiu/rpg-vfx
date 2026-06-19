
using UnityEngine;

public class PlayerWalk : PlayerState
{
    protected PlayerStates backState;

    public PlayerWalk(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PlayerStates.WALK;
        backState = PlayerStates.IDLE;
    }

    public override void Update()
    {
        base.Update();

        if (Player.Rb.linearVelocity == Vector2.zero)
            PlayerAnim.ChangeState(backState);
        else if (Player.PressDashThisFrame) 
            PlayerAnim.ChangeState(PlayerStates.DASH);
        else DirectionCheck();
    }

    protected virtual void DirectionCheck()
    {
        var a = Player.Rb.linearVelocityX / Player.Rb.linearVelocityY;
        if (a < -PlayerAnimController.VIRTUAL_DIR_RANGE || a > PlayerAnimController.VIRTUAL_DIR_RANGE)
            PlayerAnim.ChangeState(PlayerStates.WALK_SIDE);
        else if (Player.Rb.linearVelocityY > 0)
            PlayerAnim.ChangeState(PlayerStates.WALK_BACK);
    }
}

public class PlayerWalkBack : PlayerWalk
{
    public PlayerWalkBack(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PlayerStates.WALK_BACK;
        backState = PlayerStates.IDLE_BACK;
    }

    protected override void DirectionCheck()
    {
        var a = Player.Rb.linearVelocityX / Player.Rb.linearVelocityY;
        if (a < -PlayerAnimController.VIRTUAL_DIR_RANGE || a > PlayerAnimController.VIRTUAL_DIR_RANGE)
            PlayerAnim.ChangeState(PlayerStates.WALK_SIDE);
        else if (Player.Rb.linearVelocityY < 0)
            PlayerAnim.ChangeState(PlayerStates.WALK);
    }
}

public class PlayerWalkSide : PlayerWalk
{
    public PlayerWalkSide(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PlayerStates.WALK_SIDE;
        backState = PlayerStates.IDLE_SIDE;
    }

    public override void Update()
    {
        base.Update();
        CheckSide();
    }

    protected override void DirectionCheck()
    {
        var a = Player.Rb.linearVelocityX / Player.Rb.linearVelocityY;
        if (a > -PlayerAnimController.VIRTUAL_DIR_RANGE & a < PlayerAnimController.VIRTUAL_DIR_RANGE)
        {
            if (Player.Rb.linearVelocityY > 0)
                PlayerAnim.ChangeState(PlayerStates.WALK_BACK);
            else PlayerAnim.ChangeState(PlayerStates.WALK);
        }
    }

    void CheckSide()
    {
        if (Player.Rb.linearVelocityX * PlayerAnim.transform.localScale.x < 0)
            PlayerAnim.transform.localScale = new Vector2(PlayerAnim.transform.localScale.x * -1, PlayerAnim.transform.localScale.y);
    }
}