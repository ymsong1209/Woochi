using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI���� ��ų �������� ��Ÿ��
/// </summary>
[DisallowMultipleComponent]
public class SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public  Image       selected;
    public  Image       icon;
    public  Button      btn;
    public  Transform   tooltipPos;     // ���� ��ġ�� �����ϱ� ����

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
        //_skill�� null�� ��� �� skill�� �ʱ�ȭ
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
