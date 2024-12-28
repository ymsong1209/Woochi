using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI에서 스킬 아이콘을 나타냄
/// </summary>
[DisallowMultipleComponent]
public class SkillIcon : MonoBehaviour, ITooltipiable
{
    public  Image       selected;
    public  Image       icon;
    public  Button      btn;

    public Action<BaseSkill, Transform> OnShowTooltip;
    public Action OnHideTooltip;

    protected BaseSkill skill;

    private void Start()
    {
        BattleManager.GetInstance.OnFocusStart += () => SetCanInteract(false);
    }

    private void SetCanInteract(bool value)
    {
        btn.interactable = value;
    }

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

    public void ShowTooltip()
    {
        if (btn.interactable)
        {
            GameManager.GetInstance.soundManager.PlaySFX("Movement_Mouse_Edit");
            OnShowTooltip?.Invoke(skill, transform);
        }
    }

    public void HideTooltip()
    {
        OnHideTooltip?.Invoke();
    }

    #region Getter, Setter
    public BaseSkill Skill => skill;
    #endregion
}
