using System;
using System.Collections.Generic;

public enum ModifierOperator
{
    PLUS, SUBSTRACT, MULTIPLY, DIVIDE
}


// NOTE: This class is designed to be REUSED as Modifier Pool
public class StatModifier
{
    public float Timer;
    public float Duration;
    public float Value;
    public bool IsInf;

    public StatModifier(float value, bool isInf, float duration = 0)
    {
        Set(value, isInf, duration);
    }

    public void Set(float value, bool isInf, float duration = 0)
    {
        Value = value;
        IsInf = isInf;
        Duration = duration;
        Timer = duration;
    }

    public bool IsRunning() => IsInf || Timer > 0;
}

[Serializable]
public class StatModifierSet
{
    public List<StatModifier> Plus;
    public List<StatModifier> Mul;
    public Action<float> OnChange;

    public StatModifierSet()
    {
        Plus = new();
        Mul = new();
    }
}