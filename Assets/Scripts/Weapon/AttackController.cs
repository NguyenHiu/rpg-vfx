using UnityEngine;

public class AttackController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("OnTriggerEnter2D");
        if (!collision.CompareTag("Enemy")) return;

        // Debug.Log("Is Enemy");
        if (collision.TryGetComponent<ScarecrowController>(out var comp))
        {
            // Debug.Log("Enemy get hit");
            comp.GetHit();
        }
    }
}
