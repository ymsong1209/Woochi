using UnityEngine;
using UnityEngine.UI;

public class WoochiSkillSelectionUI : MonoBehaviour
{
    public SkillEvent onSkillSelected;
    [SerializeField] WoochiSkillIcon[] skillIcons = new WoochiSkillIcon[(int)SkillElement.END];

    [SerializeField] private SkillDescriptionUI skillDescriptionUI;

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
        }
    }


    public void Initialize(bool isEnable)
    {
        gameObject.SetActive(false);
        //다른 캐릭터가 우치를 공격해서 우치 ui가 활성화된 경우에는, 이후 코드 실행 안해도됨.
        if (!isEnable) return;

        //우치 차례 시작할때는 도력 초기화로직 추가
        UIManager.GetInstance.sorceryGuageUI.SetUI();
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
        
        //우치 위치에 따른 스킬 체크
        mainCharacter.CheckSkillsOnTurnStart();
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
        if (!IsSkillAbleForFormation(mainCharacterSkill))   return false;
        if (!IsSkillReceiverAble(mainCharacterSkill))   return false;
        if(mainCharacter.SorceryPoints < mainCharacterSkill.RequiredSorceryPoints) return false;
        return true;
    }
    
    /// <summary>
    /// 현재 스킬의 owner가 스킬을 시전할 수 있는 열에 있는지 확인
    /// </summary>
    bool IsSkillAbleForFormation(BaseSkill _skill)
    {
        bool isSkillSetAble;

        int skillOwnerIndex = BattleManager.GetInstance.GetCharacterIndex(_skill.SkillOwner);
        isSkillSetAble = _skill.IsSkillAvailable(skillOwnerIndex);
        return isSkillSetAble;
    }

    /// <summary>
    /// 스킬의 적용 대상이 존재하는지 확인
    /// </summary>
    bool IsSkillReceiverAble(BaseSkill _skill)
    {
        for(int i = 0; i < _skill.SkillRadius.Length; ++i)
        {
            //SingleWithoutSelf일 경우 자신을 제외한 대상이 존재하는지 확인
            if(_skill.SkillTargetType == SkillTargetType.SingularWithoutSelf && 
               i == BattleManager.GetInstance.GetCharacterIndex(_skill.SkillOwner))
                continue;
            
            if (_skill.SkillRadius[i] && BattleManager.GetInstance.IsCharacterThere(i))
                return true;
        }

        return false;
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

        // BattleManager의 SkillSelected 호출
        // SkillTriggerSelector의 Activate 메서드 호출
        onSkillSelected.Invoke(_skill);
    }


    private void SetSkillTooltip(BaseSkill skill, Transform transform)
    {
        skillDescriptionUI.Activate(skill);
        skillDescriptionUI.transform.position = transform.position + new Vector3(30, 75, 0);
    }

    public SkillEvent OnSkillSelected => onSkillSelected;
}
