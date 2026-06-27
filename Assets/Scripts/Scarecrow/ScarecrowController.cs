using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ScarecrowController : MonoBehaviour
{
    public SpriteRenderer SR;
    public float HitCooldown;
    public float Timer;
    private bool IsBlinking;

    // TODO: make the scarecrow loop left-right when being hit by rotating

    void OnDisable()
    {
        SR.DOKill();
    }

    void Update()
    {
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
        Debug.Log("Scarerow got hit");
        // if (Timer >= 0) return false;
        Timer = HitCooldown;
        IsBlinking = true;

        SR.DOKill();
        SR.DOColor(Color.red, 0.5f)
            .SetLoops(-1, LoopType.Yoyo);
        return true;
    }


}
