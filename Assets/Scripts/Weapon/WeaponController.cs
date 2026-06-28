using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class WeaponController : MonoBehaviour
{
    [field: SerializeField] public PlayerController Player {get; private set; }
    [field: SerializeField] public SpriteRenderer SR {get; private set; }
    [field: SerializeField] public Transform Head {get; private set; }
    [field: SerializeField] public StateMachine StateM {get; private set; }
    [field: SerializeField] public List<WeaponState> States {get; private set; }
    [field: SerializeField] public Animator Anim {get; private set; }
    [field: SerializeField] public float WeaponLength {get; private set; }

    [field: Header("===== Stats =====")]
    [field: Header("Idle")]
    [field: SerializeField] public float YRange {get; private set; }
    [field: SerializeField] public float IdleSpeed {get; private set; }
    [field: SerializeField] public float IdleAngle {get; private set; }
    [field: SerializeField] public Transform IdlePointRight {get; private set; }

    [field: Header("Attack")]
    [field: SerializeField] public float AttackSpeed {get; private set; }
    [field: SerializeField] public float AttackAngle {get; private set; }
    [field: SerializeField] public float AttackRadius {get; private set; }
    [field: SerializeField] public Transform AttackCenter {get; private set; }
    [field: SerializeField] public PolygonCollider2D PC2D {get; private set; }

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
}
