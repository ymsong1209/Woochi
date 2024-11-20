using UnityEngine;
using TMPro;

public class BuffPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupText; // 팝업 텍스트
    [SerializeField] private RectTransform CanvasRt;
    [SerializeField] private RectTransform PanelRt;
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Activate(BuffIcon icon)
    {
        gameObject.SetActive(true);
        UpdatePanelSize();
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(icon.transform.position);
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRt, screenPosition, null, out canvasPosition);
        PanelRt.localPosition = ClampToScreen(canvasPosition); // 팝업 위치를 마우스 위치로 설정
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    
    private void UpdatePanelSize()
    {
        Canvas.ForceUpdateCanvases();
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
