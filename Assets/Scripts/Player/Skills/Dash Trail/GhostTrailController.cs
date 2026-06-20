using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.Video;

public class GhostTrailController : MonoBehaviour
{
    public PlayerController Player;
    public SpriteRenderer PlayerSR;
    public float TrailLiveTime;
    public float Delta;
    private float m_timer;
    public int Count;
    public List<GhostTrail> TrailPool;
    public GameObject TrailPrefab;
    private int m_idx;
    public Material WhiteMat;
    public bool IsShowing;

    void Awake()
    {
        for (int i = 0; i < Count; i++)
        {
            var obj = Instantiate(TrailPrefab, transform);
            TrailPool.Add(obj.GetComponent<GhostTrail>());
            obj.SetActive(false);
        }
        IsShowing = false;
    }

    void Update()
    {
        if (!IsShowing) return;

        m_timer -= Time.deltaTime;
        if (m_timer < 0)
        {
            m_timer = Delta / Player.SpeedBuff;
            SpawnTrail();
        }
    }

    void SpawnTrail()
    {
        if (m_idx >= TrailPool.Count) m_idx = 0;
        TrailPool[m_idx].StartTrail(PlayerSR, TrailLiveTime);
        m_idx++;
    }

    public void StartTrails()
    {
        IsShowing = true;
        m_timer = 0;
    }

    public void EnoughTrails()
    {
        IsShowing = false;
        // Spawn one more trail
        SpawnTrail();
    }
}
