using UnityEngine;

public class State
{
    public string Name;
    public bool DebugLog = false;

    public State(string name = "Unknown")
    {
        Name = name;
    }

    public void SetDebugLog(bool val) { DebugLog = val; }

    public virtual void Enter()
    {
        if (DebugLog)
            Debug.Log($"[State] Enter '{Name}'");
    }
    public virtual void Update()
    {

    }
    public virtual void Exit()
    {
        if (DebugLog)
            Debug.Log($"[State] Exit '{Name}'");
    }
}