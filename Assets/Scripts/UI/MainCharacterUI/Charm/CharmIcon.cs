using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharmIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public  Image       selected;
    public  Image       icon;
    public  Button      btn;
    public  Transform   tooltipPos;     // 툴팁 위치를 지정하기 위해

    protected BaseCharm charm;

    public void SetCharm(BaseCharm _charm, bool isEnable = true)
    {
        if (_charm != null)
        {
            gameObject.SetActive(isEnable);
            icon.sprite = _charm.CharmIcon;
            charm = _charm;
        }
        //_charm null일 경우 빈 charm icon으로 초기화
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetMark(bool _isActive)
    {
        selected.gameObject.SetActive(_isActive);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (charm == null)
            return;

        UIManager.GetInstance.SetCharmToolTip(charm, tooltipPos.position);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        UIManager.GetInstance.skillDescriptionUI.Deactivate();
    }
    
    

    #region Getter, Setter
    public BaseCharm Charm => charm;
    #endregion
}
