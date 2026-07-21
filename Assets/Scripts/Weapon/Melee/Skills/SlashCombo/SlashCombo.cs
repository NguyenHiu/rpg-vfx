

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Slash combo includes 3 steps: slash on correct direction -> revert direction -> piercing
/// </summary>
public class SlashCombo : MeleeSkill
{
    /// NOTE:
    /// We need 2 different colliders for this skills: one for slash and one for piercing
    protected PolygonCollider2D m_slashCollider;
    protected PolygonCollider2D m_pierceCollider;
    protected new SlashComboCfg Cfg;

    // Skill Properties
    protected int m_stepIdx;
    protected int m_maxStep;
    protected float m_comboTimer;

    public SlashCombo(SlashComboCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
    {
        Cfg = cfg;
        m_maxStep = 3;
        InitColliders();
    }

    protected void InitColliders()
    {
        {
            var go = GameObject.Instantiate(Weapon.HitAreaPrefab, Weapon.transform);
            m_slashCollider = go.GetComponent<PolygonCollider2D>();
            m_slashCollider.gameObject.SetActive(false);
            // Create the pizza collision xD
            var rad = Cfg.SlashAngle * Mathf.Deg2Rad;
            var sinVal = Mathf.Sin(rad);
            var cosVal = Mathf.Cos(rad);
            Vector2[] points = new Vector2[]
            {
            new(0, 0),
            new(-sinVal*Weapon.WeaponLength, cosVal*Weapon.WeaponLength),
            new(0, Weapon.WeaponLength),
            new(sinVal*Weapon.WeaponLength, cosVal*Weapon.WeaponLength)
            };
            m_slashCollider.SetPath(0, points);
        }

        {
            var go = GameObject.Instantiate(Weapon.HitAreaPrefab, Weapon.transform);
            m_pierceCollider = go.GetComponent<PolygonCollider2D>();

            m_pierceCollider.gameObject.SetActive(false);
            // Create the pizza collision xD
            var rad = Cfg.PiercingAngle * Mathf.Deg2Rad;
            var sinVal = Mathf.Sin(rad);
            var cosVal = Mathf.Cos(rad);
            Vector2[] points = new Vector2[]
            {
            new(0, 0),
            new(-sinVal*Weapon.WeaponLength, cosVal*Weapon.WeaponLength),
            new(0, Weapon.WeaponLength),
            new(sinVal*Weapon.WeaponLength, cosVal*Weapon.WeaponLength)
            };
            m_pierceCollider.SetPath(0, points);
        }
    }

    public override void FixedUpdate(float dt, PlayerContext context)
    {
        base.FixedUpdate(dt, context);

        if (m_comboTimer < 0 && m_stepIdx != 0)
        {
            // Restart Combo
            m_stepIdx = 0;
        }
        
    }
}