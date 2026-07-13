using UnityEngine;

// Includes: Sword, Pierce, Axe
public class MeleeWeaponController : WeaponController, IBasicAttack
{
    [Header("Requirements")]
    [SerializeField] protected BasicAttackCfg m_basicAttackCfg;
    public BasicAttackCfg BasicAttackCfg => m_basicAttackCfg;
    [SerializeField] protected Transform m_headTf;

    [Header("View Only")]
    [SerializeField] protected float m_weaponLength;
    public float WeaponLength => m_weaponLength;
    [SerializeField] protected PolygonCollider2D m_basicAttackCollider;

    void Awake()
    {
        InitBasicAttack();
    }

    protected void InitBasicAttack()
    {
        m_weaponLength = Mathf.Abs(m_headTf.localPosition.magnitude * m_sr.transform.localScale.x);
        m_basicAttackCollider.gameObject.SetActive(false);

        // Create the pizza collision xD
        var rad = m_basicAttackCfg.Angle * Mathf.Deg2Rad;
        var sinVal = Mathf.Sin(rad);
        var cosVal = Mathf.Cos(rad);
        Vector2[] points = new Vector2[]
        {
            new(0, 0),
            new(-sinVal*m_weaponLength, cosVal*m_weaponLength),
            new(0, m_weaponLength),
            new(sinVal*m_weaponLength, cosVal*m_weaponLength)
        };
        m_basicAttackCollider.SetPath(0, points);
    }
}