using UnityEngine;
using UnityEngine.UI;

public class BlinkRed : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float time = 1.0f;
    // Update is called once per frame
    private Color originalColor;

    void Start()
    {
        originalColor = sprite.color;
    }
    
    void Update()
    {
        // 시간에 따라 알파 값을 0에서 1 사이로 변하게 함
        float red = Mathf.PingPong(Time.time / time, 1.0f);

        // 이미지의 현재 색상을 가져옴
        Color color = sprite.color;
        color.r= red;
        sprite.color = color;
    }
}
