using UnityEngine;
using UnityEngine.UI;

public class WoochiActionButton : MonoBehaviour
{
    [SerializeField] protected Image icon;
    [SerializeField] protected Sprite[] onoffSprites;
    [SerializeField] protected Toggle toggle;

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
}
