using UnityEngine;


public class DebugRect : MonoBehaviour
{
    public Vector2 Center = Vector2.zero;
    public Vector2 Size = new Vector2(1, 1);
    public Color Color = Color.green;
    public float Duration = 0; // Debug.DrawLine�� ���� �ð�

    // ���� ���� �� �簢�� �׸���
    void Update()
    {
        DrawDebugRect(Center, Size, Color, Duration);
    }

    // Gizmos�� ����Ͽ� �����Ϳ��� �簢�� �׸���
    void OnDrawGizmos()
    {
        DrawGizmoRect(Center, Size, Color);

       
    }

    // Debug.DrawLine�� ����Ͽ� �簢�� �׸���
    void DrawDebugRect(Vector2 center, Vector2 size, Color color, float duration)
    {
        // �簢���� �𼭸� ���
        Vector3[] points = GetRectPoints(center, size);

        // �𼭸��� �����Ͽ� �簢�� �׸���
        Debug.DrawLine(points[0], points[1], color, duration);
        Debug.DrawLine(points[1], points[2], color, duration);
        Debug.DrawLine(points[2], points[3], color, duration);
        Debug.DrawLine(points[3], points[0], color, duration);
    }

    // Gizmos.DrawLine�� ����Ͽ� �簢�� �׸���
    void DrawGizmoRect(Vector2 center, Vector2 size, Color color)
    {
        // Gizmos�� ���� ����
        Gizmos.color = color;

        // �簢���� �𼭸� ���
        Vector3[] points = GetRectPoints(center, size);

        // �𼭸��� �����Ͽ� �簢�� �׸���
        Gizmos.DrawLine(points[0], points[1]);
        Gizmos.DrawLine(points[1], points[2]);
        Gizmos.DrawLine(points[2], points[3]);
        Gizmos.DrawLine(points[3], points[0]);
    }

    // �簢���� �𼭸� ����Ʈ ���
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
