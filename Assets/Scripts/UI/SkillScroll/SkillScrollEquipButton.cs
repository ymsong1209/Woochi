using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillScrollEquipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Button btn;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Sprite hoveredIcon;
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        icon.sprite = hoveredIcon;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        icon.sprite = defaultIcon;
    }

    public void Reset()
    {
        icon.sprite = defaultIcon;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        icon.sprite = defaultIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Button Btn
    {
        get => btn;
    }
}
