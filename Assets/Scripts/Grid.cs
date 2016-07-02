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

    /// <summary>Adds a player to the grid at the given position.</summary>
    /// <param name="startPosition"></param>
    /// <param name="direction"></param>
    /// <param name="team"></param>
    /// <returns></returns>
    public Player AddPlayer(Point startPosition, Directions direction, Teams team) //TODO: Add sprite parameter
    {
        if(walls != null && walls[startPosition.Y, startPosition.X]) { throw new System.ArgumentException("There's a wall at the given spawn location."); }
        if(players.Any(p => p.Position.X == startPosition.X && p.Position.Y == startPosition.Y)) { throw new System.ArgumentException("There's already a player at the given spawn location."); }
        //Spawn new player
        var player = Instantiate(playerPrefab).GetComponent<Player>();
        player.Grid = this;
        player.SetInitialPosition(startPosition, direction);
        player.Team = team;
        player.transform.parent = transform;
        player.BeforeMove += PlayerBeforeMove;
        players.Add(player);
        return player;
    }

    /// <summary>Event handler, that is called, before a player moves.</summary>
    private void PlayerBeforeMove(MoveEventArguments arguments)
    {
        //Wall collision
        if(walls != null && walls[arguments.TargetPosition.Y, arguments.TargetPosition.X])
        {
            arguments.Cancel();
            arguments.Player.Die();
        }
        //Enemy player collision
        else if(players
            .Where(p => p.Team != arguments.Player.Team)
            .SelectMany(p => p.BodyPositions)
            .Any(position => position.X == arguments.TargetPosition.X && position.Y == arguments.TargetPosition.Y))
        {
            arguments.Cancel();
            arguments.Player.Die();
        }
        //Player collision
        //TODO: Get powerups, when moving over them
    }
}
