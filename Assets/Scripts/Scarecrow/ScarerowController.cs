using UnityEngine;

public class ScarerowController : MonoBehaviour
{
    public float HitCooldown;
    public float Timer;

    void Update()
    {
        if (Timer >= 0) Timer -= Time.deltaTime;
    }

    public bool GetHit()
    {
        if (Timer >= 0) return false;
        Timer = HitCooldown;

    }
    

}
