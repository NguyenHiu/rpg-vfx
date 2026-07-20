using UnityEngine.InputSystem;
using UnityEngine;

public interface IBasicAttack
{
}

public class MeleeBasicAttack : MeleeSkill
{
    // TODO: Use Cfg.Speed
    protected new MeleeBasicAttackCfg Cfg;
    protected new MeleeController Weapon;
    protected PolygonCollider2D m_collider;
    public PolygonCollider2D Collider => m_collider;

    public MeleeBasicAttack(MeleeBasicAttackCfg cfg, SkillController skillCtrl, InputAction action) : base(cfg, skillCtrl, action)
    {
        Weapon = (MeleeController)skillCtrl.Weapon;
        Cfg = cfg;

        var go = GameObject.Instantiate(Weapon.HitAreaPrefab, Weapon.transform);
        m_collider = go.GetComponent<PolygonCollider2D>();
        
        m_collider.gameObject.SetActive(false);
        // Create the pizza collision xD
        var rad = Weapon.BasicAttackCfg.Angle * Mathf.Deg2Rad;
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
        Debug.Log($"[MeleeBasicAttack] Activate");
        base.Activate();
        Weapon.AnimCtrl.ChangeState(WState.ATTACK, CompleteAttack);
    }

    public void CompleteAttack()
    {
        Weapon.AnimCtrl.ChangeState(WState.IDLE);
        Cancel();
    }
}