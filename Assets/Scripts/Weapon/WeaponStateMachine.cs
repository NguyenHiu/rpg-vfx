using System;

/// <summary>
/// WeaponStateMachine = StateMachine + Ability to pass callback into a state xDDDDD
/// </summary>
public class WeaponStateMachine : StateMachine
{
    public WeaponStateMachine(WeaponState state) : base(state)
    {

    }

    public void ChangeState(WeaponState newState, Action callback)
    {
        PreviousState = CurrentState;
        CurrentState.Exit();
        CurrentState = newState;

        var state = (WeaponState)CurrentState;
        if (callback == null) state.Enter();
        else state.EnterCb(callback);
    }
}