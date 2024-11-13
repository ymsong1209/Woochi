using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillScrollEnhanceBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SkillScrollDescription skillScrollDescription;
    [SerializeField] private int SkillID;
    private bool isEnabled;

    public void SetSkill(int skillid)
    {
        SkillID = skillid;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        skillScrollDescription.SetEnhancedSkillDescription();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        skillScrollDescription.ResetEnhancedSkillDescription();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
