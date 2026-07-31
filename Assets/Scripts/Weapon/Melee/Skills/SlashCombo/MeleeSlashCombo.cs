

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Slash combo includes 3 steps: slash on correct direction -> revert direction -> piercing
/// </summary>
public class MeleeSlashCombo : MeleeSkill
{
    /// NOTE:
    /// We need 2 different colliders for this skills: one for slash and one for piercing
    protected PolygonCollider2D m_slashCollider;
    protected PolygonCollider2D m_pierceCollider;
    protected new MeleeSlashComboCfg Cfg;

    // Skill Properties
    public int StepIdx { get; protected set; }
    protected int m_maxStep;
    protected float m_comboTimer;

    public MeleeSlashCombo(MeleeSlashComboCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
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

        if (m_comboTimer < 0 && StepIdx != 0 && !IsRunning)
        {
            // Restart Combo
            StepIdx = 0;
        }

    }

    public override void Activate()
    {
        base.Activate();
        // Debug.Log($"[MeleeSlashCombo] Activate");
        Weapon.AnimCtrl.ChangeState(WState.SLASH_COMBO, CompleteAttack);
    }

    public void CompleteAttack()
    {
        StepIdx += 1;
        if (StepIdx >= m_maxStep) StepIdx = 0;

        Weapon.AnimCtrl.ChangeState(WState.IDLE);
        Cancel();
    }

    public override void Cancel()
    {
        base.Cancel();
        // Debug.Log($"[MeleeSlashCombo] Cancel");
    }
}