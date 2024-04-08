using UnityEngine;


public class DebugRect : MonoBehaviour
{
    public Vector2 Center = Vector2.zero;
    public Vector2 Size = new Vector2(1, 1);
    public Color Color = Color.green;
    public float Duration = 0; // Debug.DrawLine의 지속 시간

    // 게임 실행 중 사각형 그리기
    void Update()
    {
        DrawDebugRect(Center, Size, Color, Duration);
    }

    // Gizmos를 사용하여 에디터에서 사각형 그리기
    void OnDrawGizmos()
    {
        DrawGizmoRect(Center, Size, Color);

       
    }

    // Debug.DrawLine을 사용하여 사각형 그리기
    void DrawDebugRect(Vector2 center, Vector2 size, Color color, float duration)
    {
        // 사각형의 모서리 계산
        Vector3[] points = GetRectPoints(center, size);

        // 모서리를 연결하여 사각형 그리기
        Debug.DrawLine(points[0], points[1], color, duration);
        Debug.DrawLine(points[1], points[2], color, duration);
        Debug.DrawLine(points[2], points[3], color, duration);
        Debug.DrawLine(points[3], points[0], color, duration);
    }

    // Gizmos.DrawLine을 사용하여 사각형 그리기
    void DrawGizmoRect(Vector2 center, Vector2 size, Color color)
    {
        // Gizmos의 색상 설정
        Gizmos.color = color;

        // 사각형의 모서리 계산
        Vector3[] points = GetRectPoints(center, size);

        // 모서리를 연결하여 사각형 그리기
        Gizmos.DrawLine(points[0], points[1]);
        Gizmos.DrawLine(points[1], points[2]);
        Gizmos.DrawLine(points[2], points[3]);
        Gizmos.DrawLine(points[3], points[0]);
    }

    // 사각형의 모서리 포인트 계산
    Vector3[] GetRectPoints(Vector2 center, Vector2 size)
    {
        Vector3[] points = new Vector3[4];
        points[0] = transform.position + new Vector3(center.x - size.x / 2, center.y + size.y / 2, 0);
        points[1] = transform.position + new Vector3(center.x + size.x / 2, center.y + size.y / 2, 0);
        points[2] = transform.position + new Vector3(center.x + size.x / 2, center.y - size.y / 2, 0);
        points[3] = transform.position + new Vector3(center.x - size.x / 2, center.y - size.y / 2, 0);
        return points;
    }


}
