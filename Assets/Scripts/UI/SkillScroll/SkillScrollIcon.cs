using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillScrollIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject selected;
    
    [SerializeField] private SkillScroll skillScroll;
    
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    private int skillID;

    public void Start()
    {
        button.onClick.AddListener(() => skillScroll.OnSkillSelected.Invoke(skillID));
    }

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
            button.interactable = false;
        }
        else
        {
            icon.sprite = skill.SkillSO.skillIcon;
            canvasGroup.interactable = true;
            button.interactable = true;
            
        }
    }
    
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (skillID == 0 || canvasGroup.interactable == false) return;
        skillScroll.OnIconHovered.Invoke(skillID);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(skillID == 0 || canvasGroup.interactable == false) return;
        skillScroll.OnIconHoverExit.Invoke(skillID);
    }

    public SkillScroll SkillScroll
    {
        get => skillScroll;
        set => skillScroll = value;
    }
    public GameObject Selected
    {
        get => selected;
    }
    public int SkillID
    {
        get => skillID;
    }
}
