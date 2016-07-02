using UnityEngine;
using System.Collections;
using System.Linq;

public class Grid : MonoBehaviour
{
    public GameObject rectangle;
    public GameObject wall;
    public int width;
    public int height;

    private GameObject wallContainer = null;
    private bool[,] walls = null;

    void Start()
    {
        //Add walls parent gameobject
        wallContainer = new GameObject();
        wallContainer.name = "Walls";
        wallContainer.transform.parent = transform;
        wallContainer.transform.localPosition = new Vector3(0, 0, 1); //Render all walls behind the grid for now
        if (walls == null) { walls = new bool[height, width]; } //Initialize with no walls
        else { SetWallLayout(walls); }
        //Create grid (rectangles)
        for(int y = 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x)
            {
                var tile = Instantiate(rectangle);
                tile.transform.parent = transform;
                tile.transform.localPosition = new Vector3(x - 0.05f, y - 0.05f);
            }
        }
    }

    /// <summary>Sets the wall layout, which is used by this grid. This method automatically creates walls at the correct locations.</summary>
    /// <param name="walls"></param>
    public void SetWallLayout(bool[,] walls)
    {
        if (walls.GetLength(0) != height || walls.GetLength(1) != width) { throw new System.ArgumentException("Dimensions of grid and wall layout must be the same"); }
        this.walls = walls;
        if(wallContainer == null) { return; }
        //Remove all old walls
        while (wallContainer.transform.childCount > 0) { Destroy(wallContainer.transform.GetChild(0)); }
        //Add new walls
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (this.walls[y, x])
                {
                    var tile = Instantiate(wall);
                    tile.transform.parent = wallContainer.transform;
                    tile.transform.localPosition = new Vector3(x, y);
                }
            }
        }
    }
}
