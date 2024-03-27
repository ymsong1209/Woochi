using System;

/// <summary>
/// 방의 위치를 나타내는 클래스
/// 2차원 좌표값을 가지며 다른 Point와 비교가 가능
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
    /// Point 끼리 비교 시 사용하는 함수
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
