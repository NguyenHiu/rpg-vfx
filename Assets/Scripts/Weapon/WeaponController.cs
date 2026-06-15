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
        EnableMoving();
        Player.OnSpeedBuffChange += SetSpeed;
    }   

    void OnDisable()
    {
        transform.DOKill();
        Player.OnSpeedBuffChange -= SetSpeed;
    } 

    void EnableMoving()
    {
        transform.DOKill();
        var pos = transform.localPosition;
        pos.y -= YRange/2f;
        transform.localPosition = pos;

        transform.DOLocalMoveY(YRange, Speed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        EnableMoving();
    }

    // void Update()
    // {
        
    // }   

    // void CheckSide()
    // {
    //     // if (Player)
    // } 
}
