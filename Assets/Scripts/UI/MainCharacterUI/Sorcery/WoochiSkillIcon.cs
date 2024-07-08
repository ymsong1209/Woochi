using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WoochiSkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private  Image       enabledIcon;
    [SerializeField] private  Image       disabledIcon;
    [SerializeField] private  Button      btn;
    [SerializeField] private  Transform   tooltipPos;     // 툴팁 위치를 지정하기 위해

    protected BaseSkill skill;
    [SerializeField] private SkillElement skillElement;

    public void SetSkill(BaseSkill _skill, bool isEnable = true)
    {
        if (_skill != null)
        {
            enabledIcon.gameObject.SetActive(true);
            btn.interactable = isEnable;
            skill = _skill;
        }
        //_skill이 null일 경우 빈 skill로 초기화
        else
        {
            enabledIcon.gameObject.SetActive(false);
            btn.interactable = false;
            skill = null;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
   {
       if (skill == null)
           return;

       UIManager.GetInstance.SetSkillToolTip(skill, tooltipPos.position);
       
      //스킬이 설정되어있고, 버튼이 활성화상태이면 도력 게이지 감소수치 미리 보여줌
      if (skill && btn.interactable)
      {
         MainCharacterSkill mainCharacterSkill = skill as MainCharacterSkill;
         if (!mainCharacterSkill)
         {
             Debug.LogError("우치 스킬이 아님");
             return;
         }
         MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
         if (!mainCharacter)
         {
             Debug.LogError("우치가 아님");
             return;
         }
         UIManager.GetInstance.SetSorceryPointUI(mainCharacter.SorceryPoints - mainCharacterSkill.RequiredSorceryPoints);
         UIManager.GetInstance.SetSorceryPointBackgroundUI(mainCharacter.SorceryPoints);
      }
   }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.GetInstance.skillTooltip.SetActive(false);
        //우치 스킬이 선택되지 않았으면 도력 게이지 다시 원래대로 회복
        if(!BattleManager.GetInstance.CurrentSelectedSkill)
        {
            MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
            if (!mainCharacter)
            {
                Debug.LogError("우치가 아님");
                return;
            }
            UIManager.GetInstance.SetSorceryPointUI(mainCharacter.SorceryPoints);
            UIManager.GetInstance.SetSorceryPointBackgroundUI(mainCharacter.SorceryPoints);
        }
    }
    
    #region Getter Setter
    public SkillElement SkillElement => skillElement;
    public BaseSkill Skill => skill;
    
    public Button Btn => btn;
    #endregion
}
