using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillScrollIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SkillScroll skillScroll;
    
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    private int skillID;

    public void Start()
    {
        button.onClick.AddListener(Click);
    }

    public void Init(int skillid)
    {
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
            if (skill.SkillSO.skillIcon)
            {
                icon.sprite = skill.SkillSO.skillIcon;
            }
            else
            {
                icon.sprite = skillScroll.NoiconImg;
            }
            
            canvasGroup.interactable = true;
            button.interactable = true;
            
        }
    }

    private void Click()
    {
        skillScroll.OnSkillSelected.Invoke(skillID);
        GameManager.GetInstance.soundManager.PlaySFX("Dosul_Click");
    }
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (skillID == 0 || canvasGroup.interactable == false) return;
        
        GameManager.GetInstance.soundManager.PlaySFX("Dosul_Mouse");
        skillScroll.OnIconHovered?.Invoke(skillID);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(skillID == 0 || canvasGroup.interactable == false) return;
        skillScroll.OnIconHoverExit?.Invoke(skillID);
    }

    public SkillScroll SkillScroll
    {
        get => skillScroll;
        set => skillScroll = value;
    }

    public int SkillID
    {
        get => skillID;
    }

}
