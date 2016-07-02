using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject rectanglePrefab;
    public GameObject wallPrefab;
    public int width;
    public int height;

    private GameObject wallContainer = null;
    private bool[,] walls = null;
    private readonly List<Player> players = new List<Player>();

    /// <summary>Readonly, duplicate list of the contained players.</summary>
    public IList<Player> Players { get { return players.ToList().AsReadOnly(); } }

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
                var tile = Instantiate(rectanglePrefab);
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
                    var tile = Instantiate(wallPrefab);
                    tile.transform.parent = wallContainer.transform;
                    tile.transform.localPosition = new Vector3(x, y);
                }
            }
        }
    }

    public Player AddPlayer(Point startPosition, Directions direction, Teams team) //TODO: Add sprite parameter
    {
        var player = Instantiate(playerPrefab).GetComponent<Player>();
        player.Grid = this;
        player.SetInitialPosition(startPosition, direction);
        player.Team = team;
        player.transform.parent = transform;
        return player;
    }
}
