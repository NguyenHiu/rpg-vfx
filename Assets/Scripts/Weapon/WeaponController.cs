using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class WeaponController : MonoBehaviour
{
    public PlayerController Player;
    public SpriteRenderer SR;
    public Transform Head;
    public StateMachine StateM;
    public List<WeaponState> States;
    public Animator Anim;
    public float WeaponLength;

    [Header("===== Stats =====")]
    [Header("Idle")]
    public float YRange;
    public float IdleSpeed;
    public float IdleAngle;
    public Transform IdlePointRight;

    [Header("Attack")]
    public float AttackSpeed;
    public float AttackAngle;
    public float AttackRadius;
    public Transform AttackCenter;
    public PolygonCollider2D PC2D;

    void Awake()
    {
        States = new()
        {
            new WeaponIdle(this, Player, "Idle"),
            new WeaponAttack(this, Player, "Attack"),
        };

        StateM = new(GetState(WState.IDLE));

        // Calculate length of the sword
        WeaponLength = Mathf.Abs(Head.transform.localPosition.magnitude * SR.transform.localScale.x);
        // Debug.Log($"Debug; WeaponLength: {WeaponLength}");

        // Create the pizza collision xD
        var rad = AttackAngle*Mathf.Deg2Rad;
        var sinVal = Mathf.Sin(rad);
        var cosVal = Mathf.Cos(rad);
        // Debug.Log($"Debug; AttackAngle: {AttackAngle}");
        // Debug.Log($"Debug; sinVal: {sinVal}; cosVal: {cosVal}");
        Vector2[] points = new Vector2[]
        {
            new(0, 0),
            new(-sinVal*WeaponLength, cosVal*WeaponLength),
            new(0, WeaponLength),
            new(sinVal*WeaponLength, cosVal*WeaponLength)
        };
        // Debug.Log($"Debug; points: {points[0]}, {points[1]}, {points[2]};");
        PC2D.SetPath(0, points);
        PC2D.gameObject.SetActive(false);
    }

    void Update()
    {
        StateM.CurrentState.Update();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        float angleStep = 360f / 10;
        Vector3 prevPoint = AttackCenter.transform.position + new Vector3(AttackRadius, 0f, 0f);

        for (int i = 1; i <= 10; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPoint = AttackCenter.transform.position + new Vector3(Mathf.Cos(angle) * AttackRadius, Mathf.Sin(angle) * AttackRadius, 0f);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if ()
    }
}
