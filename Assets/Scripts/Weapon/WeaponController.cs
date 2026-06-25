using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    public PlayerController Player;
    public SpriteRenderer SR;
    public Transform Head;
    public StateMachine StateM;
    public List<WeaponState> States;
    public Animator SlashAnim;
    public float WeaponLength;

    [Header("===== Stats =====")]
    [Header("Idle")]
    public float YRange;
    public float IdleSpeed;
    public float IdleAngle;

    [Header("Attack")]
    public float AttackSpeed;
    public Vector2 AngleRange;
    public float AttackRadius;
    public Transform AttackCenter;

    void Awake()
    {
        States = new()
        {
            new WeaponIdle(this, Player, "Idle"),
            new WeaponAttack(this, Player, "Attack"),
        };

        StateM = new(GetState(WState.IDLE));

        WeaponLength = Mathf.Sqrt(Head.localPosition.x * Head.localPosition.x + Head.localPosition.y + Head.localPosition.y);
    }

    void Update()
    {
        StateM.CurrentState.Update();
    }

    public void ChangeState(WState state)
    {
        StateM.ChangeState(GetState(state));
    }

    private WeaponState GetState(WState state)
    {
        foreach (var s in States)
        {
            if (s.Type == state) return s;
        }

        return null;
    }


}
