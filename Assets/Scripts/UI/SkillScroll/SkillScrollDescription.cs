using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class SkillScrollDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enhanceCount; //강화 남은 횟수
    [SerializeField] private Button enhanceButton; //강화 버튼
    [SerializeField] private Image elementImage; //스킬 속성 이미지
    
    [SerializeField] private TextMeshProUGUI skillName; //스킬 이름
    [SerializeField] private TextMeshProUGUI skillDescription; //스킬 설명
    [SerializeField] private TextMeshProUGUI enhancedSkillDescription; //강화시 스킬 타입
    
    void SetSkill(int skillid)
    {
        BaseSkill skill = GameManager.GetInstance.Library.GetSkill(skillid);
        //스킬 id가 만 이하면 기본 스킬
        if (skillid < 10000)
        {
            enhanceButton.gameObject.SetActive(true);
            enhancedSkillDescription.gameObject.SetActive(false);
        }
        else
        {
            enhanceButton.gameObject.SetActive(false);
            enhancedSkillDescription.gameObject.SetActive(true);
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
}
