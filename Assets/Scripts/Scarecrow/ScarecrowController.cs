using DG.Tweening;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ScarecrowController : MonoBehaviour
{
    public SpriteRenderer SR;
    public float BlinkDuration;
    public float Timer;
    private bool IsBlinking;

    // TODO: make the scarecrow loop left-right when being hit by rotating
    // TEST
    public Sequence seq;
    public float Duration;
    public float Angle;
    public float iFrameDuration;
    public float iFrameTimer;

    void OnDisable()
    {
        SR.DOKill();
    }

    void Start()
    {
        // Init sequence
        seq = DOTween.Sequence().SetAutoKill(false);
        seq.Pause();
        seq.Append(transform.DORotate(new(0, 0, -Angle), Duration));
        seq.Append(transform.DORotate(new(0, 0, Angle), Duration * 2));
        seq.Append(transform.DORotate(new(0, 0, 0), Duration));
    }

    void Update()
    {
        if (iFrameTimer >= 0) iFrameTimer -= Time.deltaTime;
        if (Timer >= 0) Timer -= Time.deltaTime;
        else if (IsBlinking)
        {
            SR.DOKill();
            IsBlinking = false;
            SR.color = Color.white;
        }
    }

    public bool GetHit()
    {
        if (iFrameTimer > 0) return false;
        Debug.Log("Scarerow got hit");
        iFrameTimer = iFrameDuration;
        Timer = BlinkDuration;
        IsBlinking = true;

        SR.DOKill();
        SR.DOColor(Color.red, 0.5f)
            .SetLoops(-1, LoopType.Yoyo);
        HitAnim();
        return true;
    }

    private void HitAnim()
    {
        seq.Restart();
    }
}
