using UnityEngine;
using DG.Tweening;

public class WeaponController : MonoBehaviour
{
    public PlayerController Player;
    public float YRange;
    [SerializeField] private float speed;
    public float Speed => speed;


    void OnEnable()
    {
        EnableMoving(speed / Player.SpeedBuff);
        Player.OnSpeedBuffChange += SetSpeedBuff;
    }

    void OnDisable()
    {
        transform.DOKill();
        Player.OnSpeedBuffChange -= SetSpeedBuff;
    }

    void EnableMoving(float s)
    {
        transform.DOKill();
        var pos = transform.localPosition;
        pos.y -= YRange / 2f;
        transform.localPosition = pos;

        transform.DOLocalMoveY(YRange, s).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void SetSpeedBuff(float speedBuff)
    {
        EnableMoving(speed / speedBuff);
    }
}
