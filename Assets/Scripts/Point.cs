using UnityEngine;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int x, int y) { this.X = x; this.Y = y; }
    public Vector3 ToVector() { return new Vector3(X, Y); }
    public static Point operator +(Point lhs, Point rhs) { return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y); }
}