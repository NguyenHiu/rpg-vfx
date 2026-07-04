
using UnityEngine;

public class PlayerIdle : PlayerState
{
    private Vector2 m_prevFacingDir;

    public PlayerIdle(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PState.IDLE;
    }

    public override void Update()
    {
        base.Update();

        if (Player.Rb.linearVelocity != Vector2.zero || m_prevFacingDir != Player.FacingDir)
        {
            m_prevFacingDir = Player.FacingDir;
            // Switch to Walk
            PState nxtState = PState.WALK;
            var a = Player.Rb.linearVelocityX / Player.Rb.linearVelocityY;
            if (a < -PlayerAnimController.VIRTUAL_DIR_RANGE || a > PlayerAnimController.VIRTUAL_DIR_RANGE)
                nxtState = PState.WALK_SIDE;
            else if (Player.FacingDir.y > 0)
                nxtState = PState.WALK_BACK;

            PlayerAnim.ChangeState(nxtState);
        }
    }
}

public class PlayerIdleSide : PlayerIdle
{
    public PlayerIdleSide(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PState.IDLE_SIDE;
    }
}

public class PlayerIdleBack : PlayerIdle
{
    public PlayerIdleBack(PlayerController player, PlayerAnimController playerAnim, string varName) : base(player, playerAnim, varName)
    {
        Type = PState.IDLE_BACK;
    }
}