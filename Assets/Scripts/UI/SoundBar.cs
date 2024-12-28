using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundBar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Slider slider;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.GetInstance.soundManager.PlaySFX("Sound_Click");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.GetInstance.soundManager.PlaySFX("Sound_Click");
    }

    public float GetValue() => slider.value;
    public void SetValue(float _value) => slider.value = _value;
}
