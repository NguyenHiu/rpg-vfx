public class PlayerState : State
{
    public PlayerStates Type { get; protected set; }
    public PlayerController Player;
    public PlayerAnimController PlayerAnim;
    public string VarName;

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