using System;
using TMPro;
using UnityEngine;

public class CustomDebug : MonoBehaviour
{
    [Header("Stats")]
    public TextMeshProUGUI DebugText;
    public PlayerController Player;
    public float RefreshDuration;
    private float m_timer;
    private float m_fps;
    private Vector2 m_pos;

    [Header("Panel")]
    public GameObject DebugBtn;
    public GameObject DebugPanel;

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
            UpdateStatsText();
        }
    }

    void UpdateStatsText()
    {
        DebugText.text = 
            $"FPS: {m_fps}\n" + 
            $"Pos: {m_pos}\n" +
            $"Speed Buff: {Player.SpeedBuff*100}%"
            ; 
    }

    public void EnableDebugPanel()
    {
        DebugBtn.SetActive(false);
        DebugPanel.SetActive(true);
    }

    public void DisableDebugPanel()
    {
        DebugBtn.SetActive(true);
        DebugPanel.SetActive(false);
    }

    public void SetSpeedBuff(float val = 1.0f)
    {
        Player.SetSpeedBuff(val);
    }
}
