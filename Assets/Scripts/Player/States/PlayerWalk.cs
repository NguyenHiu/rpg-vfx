
using UnityEngine;

public class PlayerWalk : PlayerState
{
    public PlayerWalk(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (Player.Rb.linearVelocity == Vector2.zero)
            PlayerAnim.ChangeState(PlayerStates.IDLE);
        else
        {
            var a = Player.Rb.linearVelocityX / Player.Rb.linearVelocityY;
            if (a < -PlayerAnimController.VIRTUAL_DIR_RANGE || a > PlayerAnimController.VIRTUAL_DIR_RANGE)
                PlayerAnim.ChangeState(PlayerStates.WALK_SIDE);
            else if (Player.Rb.linearVelocityY > 0)
                PlayerAnim.ChangeState(PlayerStates.WALK_BACK);
        }
    }
}

public class PlayerWalkBack : PlayerState
{
    public PlayerWalkBack(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (Player.Rb.linearVelocity == Vector2.zero)
            PlayerAnim.ChangeState(PlayerStates.IDLE_BACK);
        else
        {
            var a = Player.Rb.linearVelocityX / Player.Rb.linearVelocityY;
            if (a < -PlayerAnimController.VIRTUAL_DIR_RANGE || a > PlayerAnimController.VIRTUAL_DIR_RANGE)
                PlayerAnim.ChangeState(PlayerStates.WALK_SIDE);
            else if (Player.Rb.linearVelocityY < 0)
                PlayerAnim.ChangeState(PlayerStates.WALK);
        }
    }
}

public class PlayerWalkSide : PlayerState
{
    public PlayerWalkSide(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
    }

    public override void Update()
    {
        base.Update();
        CheckSide();

        if (Player.Rb.linearVelocity == Vector2.zero)
            PlayerAnim.ChangeState(PlayerStates.IDLE_SIDE);
        else
        {
            var a = Player.Rb.linearVelocityX / Player.Rb.linearVelocityY;
            if (a > -PlayerAnimController.VIRTUAL_DIR_RANGE & a < PlayerAnimController.VIRTUAL_DIR_RANGE)
            {
                if (Player.Rb.linearVelocityY > 0)
                    PlayerAnim.ChangeState(PlayerStates.WALK_BACK);
                else PlayerAnim.ChangeState(PlayerStates.WALK);
            }
        }
    }

    void CheckSide()
    {
        if (Player.Rb.linearVelocityX * PlayerAnim.transform.localScale.x < 0)
            PlayerAnim.transform.localScale = new Vector2(PlayerAnim.transform.localScale.x * -1, PlayerAnim.transform.localScale.y);
    }
}