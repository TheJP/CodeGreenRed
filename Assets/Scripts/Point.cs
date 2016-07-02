using UnityEngine;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int x, int y) { this.X = x; this.Y = y; }
    public Vector2 ToVector() { return new Vector2(X, Y); }
}