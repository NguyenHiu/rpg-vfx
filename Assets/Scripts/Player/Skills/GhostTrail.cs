using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostTrail : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_sr;
    private float m_liveTime;
    private float m_timer;
    private bool m_isRunning;
    private Color m_color;

    void Awake()
    {
        m_isRunning = false;
        m_sr = GetComponent<SpriteRenderer>();
        m_color = m_sr.color;
    }

    void OnDisable()
    {
        transform.DOKill();
    }

    void Update()
    {
        if (!m_isRunning) return;

        m_timer -= Time.deltaTime;
        if (m_timer < 0)
            EndTrail();
    }

    public void StartTrail(SpriteRenderer sr, float liveTime)
    {
        m_liveTime = liveTime;
        m_sr.sprite = sr.sprite;
        transform.position = sr.transform.position;
        transform.localScale = sr.transform.localScale;
        transform.rotation = sr.transform.rotation;

        m_timer = m_liveTime / 2f;
        m_isRunning = true;
        gameObject.SetActive(true);
    }

    public void EndTrail()
    {
        m_isRunning = false;
        m_sr.DOFade(0, m_liveTime / 2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            m_sr.color = m_color;
        });
    }
}
