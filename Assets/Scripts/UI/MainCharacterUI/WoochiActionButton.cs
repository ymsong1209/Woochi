using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WoochiActionButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] protected Image icon;
    [SerializeField] protected Sprite[] onoffSprites;
    [SerializeField] protected Toggle toggle;

    private void Start()
    {
        toggle.onValueChanged.AddListener(PlaySFX);
    }

    public virtual void Initialize(bool isEnable)
    {
        toggle.interactable = isEnable;
        Deactivate();
    }
    
    public virtual void Activate()
    {
        icon.sprite = onoffSprites[0];
    }

    public virtual void Deactivate()
    {
        icon.sprite = onoffSprites[1];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toggle.interactable)
        {
            AkSoundEngine.PostEvent("Movement_Mouse", gameObject);
        }
    }

    public void PlaySFX(bool isOn)
    {
        AkSoundEngine.PostEvent("Movement_Click", gameObject);
    }
}
