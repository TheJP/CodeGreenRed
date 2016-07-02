using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum Directions
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

public static class DirectionsExtensions
{
    private static Point[] movementByDirection = new[]
    {
        new Point(0, 1),
        new Point(1, 0),
        new Point(0, -1),
        new Point(-1, 0)
    };

    private static Point Movement(this Directions direction) { return movementByDirection[(int)direction]; }
}
