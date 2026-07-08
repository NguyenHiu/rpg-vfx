using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DamageFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer SR;
    [SerializeField] private float FlashDuration;
    [SerializeField] private AnimationCurve AnimCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private MaterialPropertyBlock _mpb;
    private int _flashAmountID = Shader.PropertyToID("_FlashAmount");
    private Coroutine _flashRoutine;

    void Awake()
    {
        if (SR == null)
            SR = GetComponent<SpriteRenderer>();

        _mpb = new();
    }

    public void TriggerFlash()
    {
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);
        _flashRoutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        float _timer = 0;

        while (_timer < FlashDuration)
        {
            _timer += Time.deltaTime;
            var val = AnimCurve.Evaluate(_timer / FlashDuration);
            SetFlashAmount(val);
            yield return null;
        }

        SetFlashAmount(0f);
        _flashRoutine = null;
    }

    private void SetFlashAmount(float val)
    {
        SR.GetPropertyBlock(_mpb);
        _mpb.SetFloat(_flashAmountID, val);
        SR.SetPropertyBlock(_mpb);
    }
}