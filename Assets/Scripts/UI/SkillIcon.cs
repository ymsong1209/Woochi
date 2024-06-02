using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI에서 스킬 아이콘을 나타냄
/// </summary>
[DisallowMultipleComponent]
public class SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public  Image       selected;
    public  Image       icon;
    public  Button      btn;
    public  Transform   tooltipPos;     // 툴팁 위치를 지정하기 위해

    protected BaseSkill skill;

    public void SetSkill(BaseSkill _skill, bool isEnable = true)
    {
        if (_skill != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = _skill.SkillSO.skillIcon;
            btn.interactable = isEnable;
            skill = _skill;
        }
        //_skill이 null일 경우 빈 skill로 초기화
        else
        {
            icon.gameObject.SetActive(false);
            btn.interactable = false;
            skill = null;
        }
    }

    public void SetMark(bool _isActive)
    {
        selected.gameObject.SetActive(_isActive);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (skill == null)
            return;

        UIManager.GetInstance.SetSkillToolTip(skill, tooltipPos.position);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        UIManager.GetInstance.skillTooltip.SetActive(false);
    }

    #region Getter, Setter
    public BaseSkill Skill => skill;
    #endregion
}
