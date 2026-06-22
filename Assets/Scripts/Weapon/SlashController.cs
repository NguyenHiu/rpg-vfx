using UnityEngine;

public class SlashController : MonoBehaviour
{
    public PlayerController Player;
    public float Radius;

    public void StartAttack()
    {
        Player.IsAttacking = true;

        Player.Weapon.gameObject.SetActive(false);
        transform.localEulerAngles = new(0, 0, Mathf.Atan2(Player.FacingDir.y, Player.FacingDir.x) * Mathf.Rad2Deg);
        transform.localPosition = Player.FacingDir * Radius;
    }

    public void EndAttack()
    {
        Player.IsAttacking = false;
        Player.ResetAttackTimer();
        Player.Weapon.gameObject.SetActive(true);
    }
}
