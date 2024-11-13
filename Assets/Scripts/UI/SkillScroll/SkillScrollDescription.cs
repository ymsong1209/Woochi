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
    private MainCharacterSkill curSkill;
    [SerializeField] private SkillScroll skillScroll;
    
    [SerializeField] private TextMeshProUGUI enhanceCount; //강화 남은 횟수
  
    
    [SerializeField] private Image elementImage; //스킬 속성 이미지
    [SerializeField] private Sprite[] elementImages;
    [SerializeField] private SkillScrollEnhanceBanner enhanceBanner; //강화된 스킬 아이콘
    [SerializeField] private Sprite[] enhanceBannerImages;
    
    [SerializeField] private TextMeshProUGUI skillName; //스킬 이름
    [SerializeField] private TextMeshProUGUI skillDescription; //스킬 설명
    [SerializeField] private TextMeshProUGUI enhancedSkillDescription; //강화된 스킬 설명
    
    [SerializeField] private SkillScrollEnhanceBtn enhanceButton; //강화 버튼
    [SerializeField] private Button equipButton; //스킬 장착 버튼
  

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        equipButton.onClick.AddListener(()=>
        {
            Debug.Log("Equip Skill : " + curSkillID);
            BaseSkill skill = GameManager.GetInstance.Library.GetSkill(curSkillID);
            GameManager.GetInstance.Library.EquipSkill(curSkillID,(int)skill.SkillSO.SkillElement - 1);
            skillScroll.Reset();
            skillScroll.Activate();
        });
    }
    
    public void SetSkill(int skillid)
    {
        if (skillid == 0 ||
            (curSkill = GameManager.GetInstance.Library.GetSkill(skillid) as MainCharacterSkill) == null) return;
        
        gameObject.SetActive(true);
        equipButton.gameObject.SetActive(true);

        curSkillID = skillid;
        elementImage.gameObject.SetActive(true);
        elementImage.sprite = elementImages[(int)curSkill.SkillSO.SkillElement];
        skillName.SetText(curSkill.SkillSO.SkillName);
        curSkill.SetSkillScrollDescription(skillDescription);
        //스킬 id가 만 이하면 기본 스킬
        if (skillid < 10000)
        {
            enhanceButton.gameObject.SetActive(true);
            enhanceButton.SetSkill(skillid);
        }
        else
        {
            //강화된 스킬 아이콘 보여주기
            enhanceBanner.gameObject.SetActive(true);
            enhanceBanner.BannerImg.sprite = enhanceBannerImages[(int)curSkill.SkillSO.SkillElement];
            enhanceBanner.IsAlphaBlending = false;
        }
    }

    public void Reset()
    {
        curSkillID = 0;
        curSkill = null;

        elementImage.gameObject.SetActive(false);
        elementImage.sprite = null;
        enhanceBanner.gameObject.SetActive(false);
        
        skillName.SetText("");
        skillDescription.SetText("");
        enhancedSkillDescription.gameObject.SetActive(false);
        enhanceCount.text = "";
        
        enhanceButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false); }

    
    public void SetEnhancedSkillDescription()
    {
        enhancedSkillDescription.gameObject.SetActive(true);
        curSkill.SetEnhancedSkillScrollDescription(curSkillID,enhancedSkillDescription);
    }

    public void ResetEnhancedSkillDescription()
    {
        enhancedSkillDescription.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
