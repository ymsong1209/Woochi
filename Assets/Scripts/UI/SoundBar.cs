using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundBar : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Slider slider;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.GetInstance.soundManager.PlaySFX("Sound_Click");
    }

    public Slider Slider => slider;
    public float GetValue() => slider.value;
    public void SetValue(float _value) => slider.value = _value;
}
