using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BuffPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupText; // 팝업 텍스트
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private RectTransform CanvasRt;
    [SerializeField] private RectTransform PanelRt;
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdatePanelSize(); //buff icon에서 다른 buff icon으로 마우스를 옮기면 panel size가 update가 제대로 안됨.
    }

    public void Activate(Vector3 mouseposition)
    {
        gameObject.SetActive(true);
        UpdatePanelSize();
        PanelRt.localPosition = ConvertScreenToCanvasPosition(mouseposition); // 팝업 위치를 마우스 위치로 설정
    }

    public void Deactivate()
    {
        popupText.text = "";//텍스트 초기화
        UpdatePanelSize();
        gameObject.SetActive(false);
    }
    
    private Vector3 ConvertScreenToCanvasPosition(Vector3 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRt, Input.mousePosition, mainCanvas.worldCamera,
            out Vector2 localPoint);
        return new Vector3(localPoint.x, localPoint.y, 0);
    }
    
    private void UpdatePanelSize()
    {
        // 텍스트의 렌더링된 크기를 기준으로 패널의 크기를 조정
        var textSize = popupText.GetRenderedValues(false); // false는 렌더링된 너비와 높이를 가져오기 위함
        PanelRt.sizeDelta = new Vector2(textSize.x + 20, textSize.y + 20); // 여백 추가
    }
    
    public TextMeshProUGUI PopUpText => popupText;
}