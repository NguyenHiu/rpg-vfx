using DG.Tweening;
using UnityEngine;

public class ScarecrowController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Transform m_spriteTf;
    [SerializeField] private ScarecrowCfg m_cfg;
    [SerializeField] private DamageFlash dmgFlash;

    [Header("View Only")]
    [SerializeField] private float iFrameTimer = 0;
    [SerializeField] private Sequence Seq;

    void Start()
    {
        // Init sequence
        Seq = DOTween.Sequence().SetAutoKill(false);
        Seq.Pause();
        Seq.Append(m_spriteTf.DORotate(new(0, 0, -m_cfg.Angle), m_cfg.Duration));
        Seq.Append(m_spriteTf.DORotate(new(0, 0, m_cfg.Angle), m_cfg.Duration * 2));
        Seq.Append(m_spriteTf.DORotate(new(0, 0, 0), m_cfg.Duration));
    }

    void Update()
    {
        if (iFrameTimer >= 0) iFrameTimer -= Time.deltaTime;
    }

    public bool GetHit()
    {
        if (iFrameTimer > 0) return false;
        Debug.Log("Scarerow got hit");
        iFrameTimer = m_cfg.IFrameDuration;

        dmgFlash.TriggerFlash();
        HitAnim();
        return true;
    }

    private void HitAnim()
    {
        Seq.Restart();
    }
}
