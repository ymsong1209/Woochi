using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillScrollEnhanceFinalCheck : MonoBehaviour
{
    [SerializeField] private SkillScroll skillScroll;
    [SerializeField] private int skillID;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button enhanceFinalCheckBG;
    public void SetSkill(int id)
    {
        skillID = id;
        BaseSkill skill = GameManager.GetInstance.Library.GetSkill(id);
        description.SetText(skill.SkillSO.SkillName + "을(를) 강화하시겠습니까?");
    }
    // Start is called before the first frame update
    void Start()
    {
        confirmBtn.onClick.AddListener(Confirm);
        cancelBtn.onClick.AddListener(() => gameObject.SetActive(false));
        cancelBtn.onClick.AddListener(() => enhanceFinalCheckBG.gameObject.SetActive(false));
    }

    private void Confirm()
    {
        DataCloud.playerData.realization--;
        GameManager.GetInstance.Library.SetSkillOnScroll(skillID);
        gameObject.SetActive(false);
        skillScroll.Reset();
        skillScroll.Activate();
        enhanceFinalCheckBG.gameObject.SetActive(false);
        DataCloud.SavePlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
