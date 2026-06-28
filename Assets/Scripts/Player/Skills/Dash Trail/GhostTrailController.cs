using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.Video;

public class GhostTrailController : MonoBehaviour
{
    [field: SerializeField] public PlayerController Player {get; private set;}
    [field: SerializeField] public SpriteRenderer PlayerSR {get; private set;}
    [field: SerializeField] public float TrailLiveTime {get; private set;}
    [field: SerializeField] public float Delta {get; private set;}
    private float m_timer;
    [field: SerializeField] public int Count {get; private set;}
    [field: SerializeField] public List<GhostTrail> TrailPool {get; private set;}
    [field: SerializeField] public GameObject TrailPrefab {get; private set;}
    private int m_idx;
    [field: SerializeField] public Material WhiteMat {get; private set;}
    private bool m_isShowing;

    void Awake()
    {
        for (int i = 0; i < Count; i++)
        {
            var obj = Instantiate(TrailPrefab, transform);
            TrailPool.Add(obj.GetComponent<GhostTrail>());
            obj.SetActive(false);
        }
        m_isShowing = false;
    }

    void Update()
    {
        if (!m_isShowing) return;

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
        m_isShowing = true;
        m_timer = 0;
    }

    public void EnoughTrails()
    {
        m_isShowing = false;
        // Spawn one more trail
        SpawnTrail();
    }
}
