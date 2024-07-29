using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillDescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillTxt; // 팝업 텍스트
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private RectTransform panelRt;

    [SerializeField] private RectTransform skillSideBarLeft;
    [SerializeField] private RectTransform skillSideBarRight;
    
    private Vector2 initialPanelSize;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPanelSize = panelRt.sizeDelta;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdatePanelSize(); //buff icon에서 다른 buff icon으로 마우스를 옮기면 panel size가 update가 제대로 안됨.
    }

    public void Activate(BaseSkill _skill)
    {
        gameObject.SetActive(true);
        _skill.SetSkillDescription(skillTxt);
        UpdatePanelSize();
        
    }
    
    public void Activate(BaseCharm _charm)
    {
        gameObject.SetActive(true);
        _charm.SetCharmDescription(skillTxt);
        UpdatePanelSize();
        
    }

    public void Deactivate()
    {
        UpdatePanelSize();
        gameObject.SetActive(false);
    }
    
    
    private void UpdatePanelSize()
    {
        // 텍스트의 렌더링된 크기를 기준으로 패널의 크기를 조정
        var textSize = skillTxt.GetRenderedValues(false); // false는 렌더링된 너비와 높이를 가져오기 위함

        // 패널의 높이를 초기 높이보다 줄이지 않음
        float newPanelHeight = Mathf.Max(initialPanelSize.y, textSize.y + 20); // 여백 추가

        panelRt.sizeDelta = new Vector2(textSize.x + 20, newPanelHeight); // 여백 추가

        // 패널 크기에 맞게 사이드바 위치 조정
        var panelPosition = panelRt.position;
        skillSideBarLeft.sizeDelta = new Vector2(skillSideBarLeft.sizeDelta.x, newPanelHeight);
        skillSideBarRight.sizeDelta = new Vector2(skillSideBarRight.sizeDelta.x, newPanelHeight);

        // Left Sidebar
        var leftSidebarPosition = skillSideBarLeft.transform.position;
        leftSidebarPosition = new Vector3(panelPosition.x - panelRt.sizeDelta.x / 2, leftSidebarPosition.y, leftSidebarPosition.z);
        skillSideBarLeft.transform.position = leftSidebarPosition;

        // Right Sidebar
        var rightSidebarPosition = skillSideBarRight.transform.position;
        rightSidebarPosition = new Vector3(panelPosition.x + panelRt.sizeDelta.x / 2, rightSidebarPosition.y, rightSidebarPosition.z);
        skillSideBarRight.transform.position = rightSidebarPosition;
    }

    
    public TextMeshProUGUI SkillText => skillTxt;
}
