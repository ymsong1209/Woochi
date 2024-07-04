using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WoochiSkillIcon : SkillIcon
{
    [SerializeField] private SkillElement skillElement;
    public override void OnPointerEnter(PointerEventData eventData)
   {
      base.OnPointerEnter(eventData);
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
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
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
    #endregion
}
