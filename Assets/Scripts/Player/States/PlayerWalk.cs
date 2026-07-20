
using UnityEngine;

public class PlayerWalk : PlayerState
{
    protected PState backState;

    public PlayerWalk(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PState.WALK;
        backState = PState.IDLE;
    }

    public override void Update()
    {
        base.Update();

        if (Player.RB.linearVelocity == Vector2.zero)
            PlayerAnim.ChangeState(backState);
        else DirectionCheck();
    }

    protected virtual void DirectionCheck()
    {
        var a = Player.FacingDir.x / Player.FacingDir.y;
        if (a < -PlayerAnimController.VIRTUAL_DIR_RANGE || a > PlayerAnimController.VIRTUAL_DIR_RANGE)
            PlayerAnim.ChangeState(PState.WALK_SIDE);
        else if (Player.FacingDir.y > 0)
            PlayerAnim.ChangeState(PState.WALK_BACK);
    }
}

public class PlayerWalkBack : PlayerWalk
{
    public PlayerWalkBack(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PState.WALK_BACK;
        backState = PState.IDLE_BACK;
    }

    protected override void DirectionCheck()
    {
        var a = Player.FacingDir.x / Player.FacingDir.y;
        if (a < -PlayerAnimController.VIRTUAL_DIR_RANGE || a > PlayerAnimController.VIRTUAL_DIR_RANGE)
            PlayerAnim.ChangeState(PState.WALK_SIDE);
        else if (Player.FacingDir.y < 0)
            PlayerAnim.ChangeState(PState.WALK);
    }
}

public class PlayerWalkSide : PlayerWalk
{
    public PlayerWalkSide(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PState.WALK_SIDE;
        backState = PState.IDLE_SIDE;
    }

    public override void Update()
    {
        base.Update();
        CheckSide();
    }

    protected override void DirectionCheck()
    {
        var a = Player.FacingDir.x / Player.FacingDir.y;
        if (a > -PlayerAnimController.VIRTUAL_DIR_RANGE & a < PlayerAnimController.VIRTUAL_DIR_RANGE)
        {
            if (Player.FacingDir.y > 0)
                PlayerAnim.ChangeState(PState.WALK_BACK);
            else PlayerAnim.ChangeState(PState.WALK);
        }
    }

    void CheckSide()
    {
        if (Player.FacingDir.x * PlayerAnim.transform.localScale.x < 0)
            PlayerAnim.transform.localScale = new Vector2(PlayerAnim.transform.localScale.x * -1, PlayerAnim.transform.localScale.y);
    }
}