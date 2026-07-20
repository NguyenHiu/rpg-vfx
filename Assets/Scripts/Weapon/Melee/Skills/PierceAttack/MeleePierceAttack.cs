
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleePierceAttack : MeleeSkill
{
    protected new MeleePierceAttackCfg Cfg;
    protected new MeleeController Weapon;
    protected float m_pierceTimer;
    protected Vector2 m_piercingDir;
    protected AutoTargetSkill m_targetSkill;
    protected PolygonCollider2D m_collider;
    public PolygonCollider2D Collider => m_collider;

    public MeleePierceAttack(MeleePierceAttackCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
    {
        Weapon = (MeleeController)skillCtrl.Weapon;
        Cfg = cfg;
        m_targetSkill = (AutoTargetSkill) SkillCtrl.GetSkill("Auto Target");


        var go = GameObject.Instantiate(Weapon.HitAreaPrefab, Weapon.transform);
        m_collider = go.GetComponent<PolygonCollider2D>();
        
        m_collider.gameObject.SetActive(false);
        // Create the pizza collision xD
        var rad = Weapon.PierceCfg.Angle * Mathf.Deg2Rad;
        var sinVal = Mathf.Sin(rad);
        var cosVal = Mathf.Cos(rad);
        Vector2[] points = new Vector2[]
        {
            new(0, 0),
            new(-sinVal*Weapon.WeaponLength, cosVal*Weapon.WeaponLength),
            new(0, Weapon.WeaponLength),
            new(sinVal*Weapon.WeaponLength, cosVal*Weapon.WeaponLength)
        };
        m_collider.SetPath(0, points);
    }

    public override void Activate()
    {
        base.Activate();

        m_pierceTimer = Cfg.Duration;
        m_piercingDir = Player.FacingDir;
        m_targetSkill?.SetEnable(false);
        
        Weapon.AnimCtrl.ChangeState(WState.PIERCE_ATTACK);
    }

    public void CompleteAttack()
    {
        Debug.Log($"MeleePierceAttack - Complete Attack");
        Weapon.AnimCtrl.ChangeState(WState.IDLE);
    }

    public override void FixedUpdate(float dt, PlayerContext context)
    {
        base.FixedUpdate(dt, context);

        m_pierceTimer -= dt;
        if (m_pierceTimer <= 0)
        {
            Cancel();
            return;
        }

        context.Direction = m_piercingDir;
        context.Speed = Cfg.Speed;
    }

    public override void Cancel()
    {
        base.Cancel();
        CompleteAttack();
        m_targetSkill?.SetEnable(true);
    }
}