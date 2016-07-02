using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum Directions
{
    North = 0,
    West = 1,
    South = 2,
    East = 3
}

public static class DirectionsExtensions
{
    private static Point[] movementByDirection = new[]
    {
        new Point(0, 1),
        new Point(-1, 0),
        new Point(0, -1),
        new Point(1, 0)
    };

    public static Point Movement(this Directions direction) { return movementByDirection[(int)direction]; }
    public static Directions TurnLeft(this Directions direction) { return direction == Directions.East ? Directions.North : (direction + 1); }
    public static Directions TurnRight(this Directions direction) { return direction == Directions.North ? Directions.East : (direction - 1); }
    public static float Angle(this Directions direction) { return ((int)direction) * 90; }
}
