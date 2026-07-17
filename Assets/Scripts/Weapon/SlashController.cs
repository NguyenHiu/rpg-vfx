using UnityEngine;

public class SlashController : StateMachineBehaviour
{
    private SpriteRenderer m_sr;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_sr == null)
            m_sr = animator.GetComponent<SpriteRenderer>();
        if (m_sr) m_sr.enabled = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_sr) m_sr.enabled = false;
    }
}