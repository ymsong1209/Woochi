using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillScrollIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject selected;
    
    
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Image icon;
    private int skillID;

    public void Init(int skillid)
    {
        selected.SetActive(false);
        SetSkillInfo(skillid);
    }
    public void SetSkillInfo(int id)
    {
        skillID = id;
        BaseSkill skill = GameManager.GetInstance.Library.GetSkill(id);
        if (id == 0 || skill == null)
        {
            icon.sprite = defaultIcon;
            canvasGroup.interactable = false;
        }
        else
        {
            icon.sprite = skill.SkillSO.skillIcon;
            canvasGroup.interactable = true;
        }
    }
    
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (skillID == 0 || canvasGroup.interactable == false) return;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(skillID == 0 || canvasGroup.interactable == false) return;
    }
}
