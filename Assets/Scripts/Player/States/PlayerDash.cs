
using UnityEngine;

public class PlayerDash : PlayerState
{
    private string m_varName;
    private float m_animSpeed;

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
        PlayerAnim.Anim.SetBool(m_varName, true);
        PlayerAnim.Anim.speed = 0f;
    }

    public override void Exit()
    {
        PlayerAnim.Anim.speed = m_animSpeed;
        PlayerAnim.Anim.SetBool(m_varName, false);
    }
}