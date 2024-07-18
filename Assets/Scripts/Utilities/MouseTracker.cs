using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    void Update()
    {
        // 마우스 포인터의 위치를 가져옴
        Vector3 mousePosition = Input.mousePosition;
        
        // 마우스 포인터 위치를 월드 좌표로 변환 (UI 작업이 아닌 경우)
        // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        // 마우스 포인터 위치를 로그 창에 출력
        Debug.Log("Mouse Position: " + mousePosition);
        
        // 월드 좌표로 변환한 위치를 로그 창에 출력 (UI 작업이 아닌 경우)
        // Debug.Log("World Position: " + worldPosition);
    }
}
