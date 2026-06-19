public class StateMachine
{
    public State CurrentState { get; private set; }
    public State PreviousState {get; private set; }

    public StateMachine(State state)
    {
        CurrentState = state;
        CurrentState.Enter();
    }

    public void ChangeState(State newState)
    {
        PreviousState = CurrentState;
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}