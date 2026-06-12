using UnityEngine;

public class PlayerState : State
{
    public Animator Anim;
    public string VarName;

    PlayerState() : base() {}

    public override void Enter()
    {
        Anim.SetBool(VarName, true);
    }

    public override void Exit()
    {
        Anim.SetBool(VarName, true);
    }

    public override void Update()
    {
        
    }
}