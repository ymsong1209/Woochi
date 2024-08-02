using UnityEngine;
using UnityEngine.UI;

public class WoochiActionButton : MonoBehaviour
{
    [SerializeField] protected Image icon;
    [SerializeField] protected Sprite[] onoffSprites;

    public virtual void Initialize(bool isEnable)
    {
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
