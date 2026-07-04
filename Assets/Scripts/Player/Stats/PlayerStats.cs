using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public struct ModifierInput
{
    public ModifierOperator Op;
    public float Value;
    public bool IsInf;
    public float Duration;

    public ModifierInput(ModifierOperator op, float value, bool isInf, float duration = 0)
    {
        Op = op;
        Value = value;
        IsInf = isInf;
        Duration = duration;
    }
}

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerStatsConfig m_cfg;
    [SerializeField] private StatModifierSet m_speedModifiers;
    // public ModifierSet AttackModifiers;

    void Awake()
    {
        m_speedModifiers = new();
    }

    public void AddSpeedModifier(ModifierInput modifier)
    {
        if (AddModifier(m_speedModifiers, modifier))
        {
            m_speedModifiers.OnChange?.Invoke(
                ApplyModifier(m_speedModifiers, m_cfg.WalkSpeed)
            );
        }
    }

    public float GetWalkSpeed()
    {
        return ApplyModifier(m_speedModifiers, m_cfg.WalkSpeed);
    }

    private bool AddModifier(StatModifierSet set, ModifierInput modifier)
    {
        if (modifier.Op == ModifierOperator.SUBSTRACT)
            modifier.Value *= -1;
        else if (modifier.Op == ModifierOperator.DIVIDE)
            modifier.Value = 1f / modifier.Value;

        List<StatModifier> list = null;
        if (modifier.Op == ModifierOperator.PLUS || modifier.Op == ModifierOperator.SUBSTRACT)
            list = set.Plus;
        else if (modifier.Op == ModifierOperator.MULTIPLY || modifier.Op == ModifierOperator.DIVIDE)
            list = set.Mul;
        else
        {
            Debug.LogError("Incorrect Modifier Operator");
            return false;
        }

        list.Add(new(modifier.Value, modifier.IsInf, modifier.Duration));
        return true;
    }

    private float ApplyModifier(StatModifierSet set, float orgValue)
    {
        foreach (var f in set.Plus)
            if (f.IsRunning()) orgValue += f.Value;
        foreach (var f in set.Mul)
            if (f.IsRunning()) orgValue *= f.Value;
        return orgValue;
    }
}