using UnityEngine;
using TMPro;

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
        PanelRt.anchoredPosition = ClampToScreen(ConvertScreenToCanvasPosition(mouseposition)); // 팝업 위치를 마우스 위치로 설정
    }

    public void Deactivate()
    {
        popupText.text = "";//텍스트 초기화
        UpdatePanelSize();
        gameObject.SetActive(false);
    }
    
    private Vector3 ConvertScreenToCanvasPosition(Vector3 screenPosition)
    {
        if (mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Screen Space - Overlay 모드
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRt, screenPosition, null, out localPoint);
            return localPoint;
        }
        else if (mainCanvas.renderMode == RenderMode.ScreenSpaceCamera || mainCanvas.renderMode == RenderMode.WorldSpace)
        {
            // Screen Space - Camera 모드 또는 World Space 모드
            Vector2 viewportPosition = mainCanvas.worldCamera.ScreenToViewportPoint(screenPosition);
            Vector2 worldObject_ScreenPosition = new Vector2(
                ((viewportPosition.x * CanvasRt.sizeDelta.x) - (CanvasRt.sizeDelta.x * 0.5f)),
                ((viewportPosition.y * CanvasRt.sizeDelta.y) - (CanvasRt.sizeDelta.y * 0.5f)));

            return worldObject_ScreenPosition;
        }

        return Vector2.zero;
    }
    
    private void UpdatePanelSize()
    {
        // 텍스트의 렌더링된 크기를 기준으로 패널의 크기를 조정
        var textSize = popupText.GetRenderedValues(false); // false는 렌더링된 너비와 높이를 가져오기 위함
        
        // 텍스트의 extra margin 값을 가져옴
        var margin = popupText.margin;
        
        // 여백을 포함한 텍스트 크기를 계산
        var totalWidth = textSize.x + margin.x + margin.z; // left + right margin
        var totalHeight = textSize.y + margin.y + margin.w; // top + bottom margin

        // 패널 크기를 여백을 포함하여 조정
        PanelRt.sizeDelta = new Vector2(totalWidth, totalHeight); // 추가 여백 포함
    }
    
    private Vector3 ClampToScreen(Vector3 position)
    {
        // 캔버스 크기를 기준으로 제한
        float halfWidth = PanelRt.sizeDelta.x / 2f;
        float halfHeight = PanelRt.sizeDelta.y / 2f;

        float minX = -CanvasRt.sizeDelta.x / 2f + halfWidth;
        float maxX = CanvasRt.sizeDelta.x / 2f - halfWidth;
        float minY = -CanvasRt.sizeDelta.y / 2f + halfHeight;
        float maxY = CanvasRt.sizeDelta.y / 2f - halfHeight;

        // 제한된 위치 반환
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }
    
    public TextMeshProUGUI PopUpText => popupText;
}
