using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WoochiActionButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] protected Image icon;
    [SerializeField] protected Toggle toggle;

    private Coroutine fadeCoroutine;
    
    private void Start()
    {
        toggle.onValueChanged.AddListener(PlaySFX);
        BattleManager.GetInstance.OnFocusStart += () => Initialize(false);
    }

    public void Initialize(bool isEnable)
    {
        toggle.interactable = isEnable;
        if (isEnable)
        {
            FadeToAlpha(1f, 0.5f); // 천천히 알파값 1로 증가
        }
    }
    
    public virtual void Activate()
    {
    }

    public virtual void Deactivate()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toggle.interactable)
        {
            GameManager.GetInstance.soundManager.PlaySFX("Movement_Mouse_Edit");
        }
    }

    public void PlaySFX(bool isOn)
    {
        if (isOn)
        {
            GameManager.GetInstance.soundManager.PlaySFX("Movement_Click_Edit");
        }
    }
    
    private void FadeToAlpha(float targetAlpha, float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeCoroutine(targetAlpha, duration));
    }
    
    private IEnumerator FadeCoroutine(float targetAlpha, float duration)
    {
        float startAlpha = icon.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(targetAlpha);
    }

    public void SetAlpha(float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }
    
    public bool IsOn => toggle.isOn;
}
