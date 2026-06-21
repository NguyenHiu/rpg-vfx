using UnityEngine;

public class SlashController : MonoBehaviour
{
    public PlayerController Player;

    public void StartAttack()
    {
        Player.IsAttacking = true;
    }

    public void EndAttack()
    {
        Player.IsAttacking = false;
    }
}
