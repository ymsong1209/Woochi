using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WoochiActionButton : MonoBehaviour
{
    [SerializeField] private WoochiButtonList owner;
    [SerializeField] private Image icon;

    public virtual void Initialize(bool isEnable)
    {
        Interactable(isEnable);
    }
    
    public virtual void Activate()
    {
       
    }

    public virtual void Deactivate()
    {
        icon.DOColor(Color.white, 0f);
        Interactable(false);
    }

    public void Highlight()
    {
        icon.DOColor(Color.black, 0.5f);
    }
    public void DeHighlight()
    {
        icon.DOColor(Color.white, 0.5f);
    }

    protected void Interactable(bool isEnable)
    {
        icon.GetComponent<Button>().interactable = isEnable;
    }
}
