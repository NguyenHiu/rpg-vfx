using DG.Tweening;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ScarecrowController : MonoBehaviour
{
    [SerializeField] private Transform SpriteTf;

    // TEST
    [SerializeField] private Sequence Seq;
    [SerializeField] private float Duration;
    [SerializeField] private float Angle;
    [SerializeField] private float iFrameDuration;
    [SerializeField] private float iFrameTimer;
    [SerializeField] private DamageFlash dmgFlash;

    void OnDisable()
    {
    }

    void Start()
    {
        // Init sequence
        Seq = DOTween.Sequence().SetAutoKill(false);
        Seq.Pause();
        Seq.Append(SpriteTf.DORotate(new(0, 0, -Angle), Duration));
        Seq.Append(SpriteTf.DORotate(new(0, 0, Angle), Duration * 2));
        Seq.Append(SpriteTf.DORotate(new(0, 0, 0), Duration));
    }

    void Update()
    {
        if (iFrameTimer >= 0) iFrameTimer -= Time.deltaTime;
    }

    public bool GetHit()
    {
        if (iFrameTimer > 0) return false;
        Debug.Log("Scarerow got hit");
        iFrameTimer = iFrameDuration;

        dmgFlash.TriggerFlash();
        HitAnim();
        return true;
    }

    private void HitAnim()
    {
        Seq.Restart();
    }
}
