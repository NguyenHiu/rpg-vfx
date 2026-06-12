using System;
using TMPro;
using UnityEngine;

public class Debug : MonoBehaviour
{
    public TextMeshProUGUI DebugText;
    public PlayerController Player;
    public float RefreshDuration;
    private float m_timer;
    private float m_fps;
    private Vector2 m_pos;

    void Awake()
    {
        Application.targetFrameRate = 120;  // or Screen.currentResolution.refreshRate
        QualitySettings.vSyncCount = 0;     // already 0, but safe to be explicit
    }

    void Update()
    {
        m_fps = (float)Math.Round(100.0f / Time.unscaledDeltaTime) / 100.0f;
        m_pos = Player.transform.position;

        m_timer -= Time.deltaTime;
        if (m_timer < 0)
        {
            m_timer = RefreshDuration;
            DebugText.text = m_fps + "\n" + m_pos.ToString();
        }
    }
}
