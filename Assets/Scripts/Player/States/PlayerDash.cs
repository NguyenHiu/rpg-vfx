
using UnityEngine;

public class PlayerDash : PlayerState
{
    private string m_varName;
    private float m_animSpeed;
    private float m_timer;

    public PlayerDash(PlayerController player, PlayerAnimController playerAnim) : base(player, playerAnim, "")
    {
        Type = PState.DASH;
    }

    public override void Enter()
    {
        // Get info
        m_varName = ((PlayerState)PlayerAnim.StateM.PreviousState).VarName;
        m_animSpeed = PlayerAnim.Anim.speed;

        // Assign new
        Player.GhostTrailCtrl.StartTrails();
        Player.ResetDashTimer();
        PlayerAnim.Anim.SetBool(m_varName, true);
        PlayerAnim.Anim.speed = 0f;
        Player.SetDashing(true);
        m_timer = 0;
    }

    public override void Exit()
    {
        Player.GhostTrailCtrl.EnoughTrails();
        PlayerAnim.Anim.speed = m_animSpeed;
        PlayerAnim.Anim.SetBool(m_varName, false);
        Player.SetDashing(false);
    }

    public override void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > Player.DashTime / Player.SpeedBuff)
        {
            var prvState = (PlayerState)PlayerAnim.StateM.PreviousState;
            switch (prvState.Type)
            {
                case PState.WALK:
                    PlayerAnim.ChangeState(PState.IDLE);
                    break;
                case PState.WALK_SIDE:
                    PlayerAnim.ChangeState(PState.IDLE_SIDE);
                    break;
                case PState.WALK_BACK:
                    PlayerAnim.ChangeState(PState.IDLE_BACK);
                    break;

                default:
                    PlayerAnim.ChangeState(PState.IDLE);
                    break;
            }
        }
    }
}