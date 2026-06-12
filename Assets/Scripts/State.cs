using UnityEngine;

public abstract class State
{
    public string Name;
    
    public State(string name="Unknown")
    {
        Name = name;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}