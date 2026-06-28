public enum PState
{
    IDLE,
    IDLE_SIDE,
    IDLE_BACK,
    WALK,
    WALK_SIDE,
    WALK_BACK,
    DASH,
}

public class PlayerState : State
{
    public PState Type { get; protected set; }
    public PlayerController Player {get; protected set; } 
    public PlayerAnimController PlayerAnim {get; protected set; }
    public string VarName {get; protected set; }

    public PlayerState(PlayerController player, PlayerAnimController playerAnim, string varName) : base(varName)
    {
        Player = player;
        PlayerAnim = playerAnim;
        VarName = varName;
    }

    public override void Enter()
    {
        base.Enter();
        PlayerAnim.Anim.SetBool(VarName, true);
    }

    public override void Exit()
    {
        base.Exit();
        PlayerAnim.Anim.SetBool(VarName, false);
    }

    public override void Update()
    {
        base.Update();
    }
}