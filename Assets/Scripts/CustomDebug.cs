using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CustomDebug : MonoBehaviour
{
    public GameObject DebugArrow;
    public float ArrowRadius;

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
    public CanvasGroup PanelGroup;

    [Header("Auto Focus")]
    public Sprite TickText;
    public Sprite UntickText;
    public Image ToggleImg;

    void OnDisable()
    {
        PanelGroup.DOKill();
    }

    void Awake()
    {
        Application.targetFrameRate = 120;  // or Screen.currentResolution.refreshRate
        QualitySettings.vSyncCount = 0;     // already 0, but safe to be explicit
    }

    void Start()
    {
        ToggleAutoFocus(true);
    }

    void Update()
    {
        float currentFps = 1.0f / Time.unscaledDeltaTime;
        m_fps = Mathf.Round(Mathf.Lerp(m_fps, currentFps, 4f * Time.unscaledDeltaTime));
        m_pos = Player.transform.position;

        m_timer -= Time.deltaTime;
        if (m_timer < 0)
        {
            m_timer = RefreshDuration;
            UpdateStatsText();
        }

        RotateArrow();
    }

    void UpdateStatsText()
    {
        DebugText.text =
            $"FPS: {m_fps}\n" +
            $"Pos: {m_pos}\n" 
            // +
            // $"Speed Buff: {Player.SpeedBuff * 100}%"
            ;
    }

    public void EnableDebugPanel()
    {
        DebugBtn.SetActive(false);
        PanelGroup.alpha = 0;
        PanelGroup.interactable = false;

        DebugPanel.SetActive(true);
        PanelGroup.DOKill();
        PanelGroup.DOFade(1f, 0.05f).OnComplete(() =>
        {
            PanelGroup.interactable = true;
        });
    }

    public void DisableDebugPanel()
    {
        PanelGroup.interactable = false;
        PanelGroup.DOKill();
        PanelGroup.DOFade(0f, 0.05f).OnComplete(() =>
        {
            DebugPanel.SetActive(false);
            DebugBtn.SetActive(true);
        });

    }

    void RotateArrow()
    {
        DebugArrow.transform.localEulerAngles = new(0, 0, Mathf.Atan2(Player.FacingDir.y, Player.FacingDir.x) * Mathf.Rad2Deg);
        DebugArrow.transform.localPosition = Player.FacingDir * ArrowRadius;
    }

    public void ToggleAutoFocus(bool isForcedUntick)
    {
        if (isForcedUntick || Player.Mode == PlayerMode.ATTACK)
        {
            Player.ExitAttackMode();
            ToggleImg.sprite = UntickText;
            Debug.Log("Exit Attack Mode");
        }
        else if (Player.Mode == PlayerMode.FREE)
        {
            Player.EnterAttackMode();
            ToggleImg.sprite = TickText;
            Debug.Log("Enter Attack Mode");
        }
    }
}
