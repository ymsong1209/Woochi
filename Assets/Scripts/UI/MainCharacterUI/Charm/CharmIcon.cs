using System;
using UnityEngine;
using UnityEngine.UI;

public class CharmIcon : MonoBehaviour, ITooltipiable
{
    public Action<BaseCharm, Transform> OnShowTooltip;
    public Action OnHideTooltip;

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

    public void ShowTooltip()
    {
        if (charm == null)
            return;

        OnShowTooltip.Invoke(charm, transform);
    }

    public void HideTooltip()
    {
        OnHideTooltip.Invoke();
    }

    #region Getter, Setter
    public BaseCharm Charm => charm;
    #endregion
}
