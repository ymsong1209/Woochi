using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnSFX : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private string cursorSFX;
    [SerializeField] private string clickSFX;
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cursorSFX == string.Empty) return;
        AkSoundEngine.PostEvent(cursorSFX, gameObject);
    }
    
    public void OnClick()
    {
        if (clickSFX == string.Empty) return;
        AkSoundEngine.PostEvent(clickSFX, gameObject);
    }
}
