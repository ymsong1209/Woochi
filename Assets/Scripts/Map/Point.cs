using System;

/// <summary>
/// ���� ��ġ�� ��Ÿ���� Ŭ����
/// 2���� ��ǥ���� ������ �ٸ� Point�� �񱳰� ����
/// </summary>
public class Point : IEquatable<Point>
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Point ���� �� �� ����ϴ� �Լ�
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Point other)
    {
        if(ReferenceEquals(null, other)) return false;
        if(ReferenceEquals(this, other)) return true;
        return x == other.x && y == other.y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Point) obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}
