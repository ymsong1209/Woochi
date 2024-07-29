using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UnityEngine.UI 네임스페이스를 추가합니다.

public class OpenMapUI : MonoBehaviour
{
    public Button myButton;

    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick);
    }

    // 버튼이 클릭되었을 때 호출될 함수입니다.
    void OnButtonClick()
    {
        MapManager.GetInstance.view.FadeInOut(true);
        gameObject.SetActive(false);
    }
}
