using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum WallLayouts
{
    NoWalls,
    Border
}

public static class WallLayoutsExtension
{
    public static bool[,] CreateArray(this WallLayouts layout, int width, int height)
    {
        switch (layout)
        {
            case WallLayouts.NoWalls:
                return new bool[height, width];
            case WallLayouts.Border:
                var walls = new bool[height, width];
                for(int i = 0; i < width; ++i)
                {
                    walls[0, i] = true;
                    walls[height - 1, i] = true;
                }
                for (int i = 0; i < height; ++i)
                {
                    walls[i, 0] = true;
                    walls[i, width - 1] = true;
                }
                return walls;
            default:
                throw new ArgumentException("Unkown or unimplemented wall layout: " + layout.ToString());
        }
    }
}
