using UnityEngine;
using UnityEngine.EventSystems;

public class TapOutsideOverlay : MonoBehaviour, IPointerDownHandler
{
    public CustomDebug CustomDB;
    public bool EnableOnStart;

    void Awake()
    {
        if (EnableOnStart) CustomDB.EnableDebugPanel();
        else CustomDB.DisableDebugPanel();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CustomDB.DisableDebugPanel();
    }
}
