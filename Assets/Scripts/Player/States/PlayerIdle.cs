
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PlayerStates.IDLE;
    }

    public override void Update()
    {
        base.Update();

        if (Player.Rb.linearVelocity != Vector2.zero)
        {
            // Switch to Walk
            PlayerStates nxtState = PlayerStates.WALK;
            var a = Player.Rb.linearVelocityX / Player.Rb.linearVelocityY;
            if (a < -PlayerAnimController.VIRTUAL_DIR_RANGE || a > PlayerAnimController.VIRTUAL_DIR_RANGE)
                nxtState = PlayerStates.WALK_SIDE;
            else if (Player.Rb.linearVelocityY > 0)
                nxtState = PlayerStates.WALK_BACK;

            PlayerAnim.ChangeState(nxtState);
        }
    }
}

public class PlayerIdleSide : PlayerIdle
{
    public PlayerIdleSide(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PlayerStates.IDLE_SIDE;
    }
}

public class PlayerIdleBack : PlayerIdle
{
    public PlayerIdleBack(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PlayerStates.IDLE_BACK;
    }
}