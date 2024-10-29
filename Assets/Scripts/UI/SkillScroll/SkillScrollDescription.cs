using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

/// <summary>
/// 도술 두루마리에서 스킬 Icon을 눌렀을때 설명을 보여주는 UI
/// </summary>
public class SkillScrollDescription : MonoBehaviour
{
    private int curSkillID;
    
    [SerializeField] private TextMeshProUGUI enhanceCount; //강화 남은 횟수
    [SerializeField] private Button enhanceButton; //강화 버튼
    
    [SerializeField] private Image elementImage; //스킬 속성 이미지
    [SerializeField] private Image[] elementImages; //스킬 속성 이름
    
    [SerializeField] private TextMeshProUGUI skillName; //스킬 이름
    [SerializeField] private TextMeshProUGUI skillDescription; //스킬 설명
  
    void SetSkill(int skillid)
    {
        BaseSkill skill = GameManager.GetInstance.Library.GetSkill(skillid);
        skill.SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        if (skill == null)
        {
            Debug.Log("SkillScrollDescription : 스킬이 없습니다.");
            return;
        }

        curSkillID = skillid;
        elementImage = elementImages[(int)skill.SkillSO.SkillElement];
        skillName.SetText(skill.SkillSO.SkillName);
        
        //스킬 id가 만 이하면 기본 스킬
        if (skillid < 10000)
        {
            enhanceButton.gameObject.SetActive(true);
        }
        else
        {
            enhanceButton.gameObject.SetActive(false);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //강화 아이콘에 마우스를 올리면 강화 설명을 보여줌
    void SetEnhancedSkillDescription()
    {
        
    }
}
