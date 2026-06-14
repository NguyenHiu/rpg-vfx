using UnityEngine;

public class State
{
    public string Name;
    
    public State(string name="Unknown")
    {
        Name = name;
    }

    public virtual void Enter() {
        Debug.Log($"[State] Enter '{Name}'");
    }
    public virtual void Update() {

    }
    public virtual void Exit() {
        Debug.Log($"[State] Exit '{Name}'");
    }
}