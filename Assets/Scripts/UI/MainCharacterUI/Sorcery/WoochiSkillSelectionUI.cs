using UnityEngine;
using UnityEngine.UI;

public class WoochiSkillSelectionUI : MonoBehaviour
{
    public SkillEvent onSkillSelected;
    [SerializeField] WoochiSkillIcon[] skillIcons = new WoochiSkillIcon[(int)SkillElement.END];

    [SerializeField] private SkillDescriptionUI skillDescriptionUI;
    [SerializeField] private BuffDescriptionUI buffDescriptionUI;

    public void Start()
    {
        for (int i = 0; i < skillIcons.Length; i++)
        {
            int index = i;
            //Default 버튼은 없을 예정이므로 Continue;
            if (!skillIcons[i]) continue;
            Button btn = skillIcons[i].Btn;
            btn.onClick.AddListener(() => SkillButtonClicked(skillIcons[index].Skill));

            skillIcons[i].OnShowTooltip += SetSkillTooltip;
            skillIcons[i].OnHideTooltip += () => skillDescriptionUI.gameObject.SetActive(false);
            skillIcons[i].OnHideTooltip += () => buffDescriptionUI.gameObject.SetActive(false);
        }
    }
    
    public void Activate()
    {
        gameObject.SetActive(true);
        
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if (!mainCharacter)
        {
            Debug.LogError("우치 차례가 아님");
            return;
        }
        
        UIManager.GetInstance.sorceryGuageUI.SetUI();
        
        //모든 스킬 비활성화
        DisableSkills();

        #region 스킬 아이콘 Enable, Disable 설정
       
        //각 버튼 오브젝트의 SkillElement에 맞춰서 스킬 세팅
        for(int i = 0;i<mainCharacter.MainCharacterSkills.Length;++i)
        {
            BaseSkill skill = mainCharacter.MainCharacterSkills[i];
            if (!skill) continue;
            //스킬에 속성이 설정 안되어있을 경우 예외처리
            if (skill.SkillSO.SkillElement == SkillElement.Defualt || skill.SkillSO.SkillElement == SkillElement.END)
            {
                Debug.LogError($"{skill.SkillSO.SkillName}의 스킬의 element가 None임");
                continue;
            }
            //skillicon을 순회하면서 같은 element의 skill이 있으면 그곳에 할당
            for (int j = 0; j < skillIcons.Length; ++j)
            {
                WoochiSkillIcon woochiskillIcon = skillIcons[j];
                if (woochiskillIcon && woochiskillIcon.SkillElement == skill.SkillSO.SkillElement)
                {
                    //만약 이미 스킬이 할당되어있는 경우 예외처리
                    if (woochiskillIcon.Skill)
                    {
                        Debug.LogError(skill.SkillSO.SkillName + "와 같은 속성의 스킬이 이미 할당되어있음");
                        break;
                    }

                    skillIcons[j].SetSkill(skill, (IsSkillSetAvailable(skill)));
                    break;
                }
            }
        }
        
        #endregion
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
        UIManager.GetInstance.sorceryGuageUI.Restore();
    }
    
    /// <summary>
    /// 스킬 슬롯에 스킬 버튼이 활성화가 되는지 확인
    /// </summary>
    bool IsSkillSetAvailable(BaseSkill skill)
    {
        MainCharacter mainCharacter = skill.SkillOwner as MainCharacter;
        MainCharacterSkill mainCharacterSkill = skill as MainCharacterSkill;
        if(mainCharacter == null || mainCharacterSkill == null) return false;
        
        return mainCharacter.IsSkillAvailable(mainCharacterSkill) &&
               mainCharacter.SorceryPoints >= mainCharacterSkill.RequiredSorceryPoints;
    }
    
    private void DisableSkills()
    {
        foreach (WoochiSkillIcon icon in skillIcons)
        {
            if (icon)
            {
                icon.SetSkill(null);
            }
        }
    }
    
    public void SkillButtonClicked(BaseSkill _skill)
    {
        if (_skill == null)
            return;
        
        ScenarioManager.GetInstance.NextPlot(PlotEvent.Click);
        // BattleManager의 SkillSelected 호출
        // SkillTriggerSelector의 Activate 메서드 호출
        onSkillSelected.Invoke(_skill);
    }


    private void SetSkillTooltip(BaseSkill skill, Transform tr)
    {
        skillDescriptionUI.Activate(skill);
        
        RectTransform targetRt = tr as RectTransform;
        RectTransform tooltipRt = skillDescriptionUI.transform as RectTransform;
        Vector2 offset = new Vector2(0, targetRt.rect.height);
        UIManager.GetInstance.SetTooltipPosition(targetRt, tooltipRt, offset);
        
        buffDescriptionUI.Activate(skill);
        buffDescriptionUI.SkillDescriptionUI = skillDescriptionUI;
    }

    public SkillEvent OnSkillSelected => onSkillSelected;
}
