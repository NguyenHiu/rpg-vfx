using DG.Tweening;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ScarecrowController : MonoBehaviour
{
    [field: SerializeField] public SpriteRenderer SR {get; private set; }
    [field: SerializeField] public float BlinkDuration {get; private set; }
    [field: SerializeField] public float Timer {get; private set; }
    private bool IsBlinking;

    // TODO: make the scarecrow loop left-right when being hit by rotating
    // TEST
    [field: SerializeField] public Sequence Seq {get; private set; }
    [field: SerializeField] public float Duration {get; private set; }
    [field: SerializeField] public float Angle {get; private set; }
    [field: SerializeField] public float iFrameDuration {get; private set; }
    [field: SerializeField] public float iFrameTimer {get; private set; }

    void OnDisable()
    {
        SR.DOKill();
    }

    void Start()
    {
        // Init sequence
        Seq = DOTween.Sequence().SetAutoKill(false);
        Seq.Pause();
        Seq.Append(transform.DORotate(new(0, 0, -Angle), Duration));
        Seq.Append(transform.DORotate(new(0, 0, Angle), Duration * 2));
        Seq.Append(transform.DORotate(new(0, 0, 0), Duration));
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
        Seq.Restart();
    }
}
