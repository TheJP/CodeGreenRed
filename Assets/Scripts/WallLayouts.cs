using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum WallLayouts
{
    NoWalls,
    Border,
    /// <summary>Should used for maps that are at least 10 by 10.</summary>
    Corners
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
            case WallLayouts.Corners:
                var corners = new bool[height, width];
                for(int i = 0; i < 4; ++i)
                {
                    //Horizontals
                    corners[3, i] = true;
                    corners[3, width - 1 - i] = true;
                    corners[height - 4, i] = true;
                    corners[height - 4, width - 1 - i] = true;
                    //Verticals
                    corners[i, 3] = true;
                    corners[height - 1 - i, 3] = true;
                    corners[i, width - 4] = true;
                    corners[height - 1 - i, width - 4] = true;
                }
                return corners;
            default:
                throw new ArgumentException("Unkown or unimplemented wall layout: " + layout.ToString());
        }
    }
}
