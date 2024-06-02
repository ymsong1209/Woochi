using UnityEngine;
using UnityEngine.UI;

public class Alphablend : MonoBehaviour
{ 
   [SerializeField] private Image image;
   [SerializeField] private float time = 1.0f;
    // Update is called once per frame
    void Update()
    {
        // 시간에 따라 알파 값을 0에서 1 사이로 변하게 함
        float alpha = Mathf.PingPong(Time.time / time, 1.0f);

        // 이미지의 현재 색상을 가져옴
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
