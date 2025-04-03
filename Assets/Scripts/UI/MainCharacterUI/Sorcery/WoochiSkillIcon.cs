using System;
using UnityEngine;
using UnityEngine.UI;

public class WoochiSkillIcon : MonoBehaviour, ITooltipiable
{
    public Action<BaseSkill, Transform> OnShowTooltip;
    public Action OnHideTooltip;

    [SerializeField] private  Image       Icon;
    [SerializeField] private  Button      btn;

    private BaseSkill skill;
    private void Start()
    {
        BattleManager.GetInstance.OnFocusStart += () =>
        {
            btn.interactable = false;
        };
    }
    
    public void SetSkill(BaseSkill _skill, bool isEnable = true)
    {
        if (_skill != null)
        {
            skill = _skill;
            Icon.gameObject.SetActive(true);
            Icon.sprite = skill.SkillSO.skillIcon;
            btn.interactable = isEnable;
        }
        //_skill이 null일 경우 빈 skill로 초기화
        else
        {
            Icon.gameObject.SetActive(false);
            btn.interactable = false;
            skill = null;
        }
    }

    public void ShowTooltip()
    {
        if (skill == null || btn.interactable == false)
            return;

        OnShowTooltip.Invoke(skill, transform);

        //스킬이 설정되어있고, 버튼이 활성화상태이면 도력 게이지 감소수치 미리 보여줌
        if (skill && btn.interactable)
        {
            MainCharacterSkill mainCharacterSkill = skill as MainCharacterSkill;
            if (!mainCharacterSkill)
            {
                Debug.LogError("우치 스킬이 아님");
                return;
            }

            UIManager.GetInstance.sorceryGuageUI.ShowAnimation(mainCharacterSkill.RequiredSorceryPoints, true);
        }
    }

    public void HideTooltip()
    {
        OnHideTooltip.Invoke();

        BaseSkill selectedSkill = BattleManager.GetInstance.CurrentSelectedSkill;
        //우치 스킬이 선택되지 않았으면 도력 게이지 다시 원래대로 회복
        if (!selectedSkill || btn.interactable == false)
        {
            UIManager.GetInstance.sorceryGuageUI.Restore();
        }
    }

    #region Getter Setter
    public BaseSkill Skill => skill;
    
    public Button Btn => btn;
    #endregion
}
