using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillScrollIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Sprite icon;
    [SerializeField] private int skillID;

    public void SetSkillInfo(int id)
    {
        skillID = id;
        BaseSkill skill = GameManager.GetInstance.Library.GetSkill(id);
        if (skill == null)
        {
            icon = defaultIcon;
        }
        else
        {
            icon = skill.SkillSO.skillIcon;
        }
    }
    
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // if (skill == null || !canInteract)
        //     return;
        //
        // UIManager.GetInstance.SetSkillToolTip(skill, tooltipPos.position);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //UIManager.GetInstance.skillDescriptionUI.Deactivate();
    }
}
