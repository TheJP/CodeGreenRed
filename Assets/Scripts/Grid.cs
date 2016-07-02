using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    private const int MaxPowerupSpawnTries = 100;

    public GameObject playerPrefab;
    public GameObject rectanglePrefab;
    public GameObject wallPrefab;
    public GameObject[] powerupPrefabs;
    public int width;
    public int height;

    private GameObject wallContainer = null;
    private GameObject powerupContainer = null;
    private bool[,] walls = null;
    private readonly List<Player> players = new List<Player>();
    private readonly List<Powerup> powerups = new List<Powerup>();

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
        //Add powerup parent gameobject
        powerupContainer = new GameObject();
        powerupContainer.name = "Powerups";
        powerupContainer.transform.parent = transform;
        powerupContainer.transform.localPosition = new Vector3(0, 0, -2); //Render all powerups in front of the grid
        //Create grid (rectangles)
        for (int y = 0; y < height; ++y)
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
        if (
            //Wall collision
            (walls != null && walls[arguments.TargetPosition.Y, arguments.TargetPosition.X]) ||
            //Enemy player collision
            players.Where(p => p.Team != arguments.Player.Team)
            .SelectMany(p => p.BodyPositions)
            .Any(position => position.X == arguments.TargetPosition.X && position.Y == arguments.TargetPosition.Y)
        ){
            arguments.Cancel();
            arguments.Player.Die();
        }
        //Canceled somewhere else?
        if (arguments.Canceled) { return; }
        //Pickup all powerups on the target field
        foreach(var powerup in powerups.Where(up => up.Position.X == arguments.TargetPosition.X && up.Position.Y == arguments.TargetPosition.Y).ToList())
        {
            powerup.PickedUp(new PickupParameters(arguments.Player, this));
            powerups.Remove(powerup);
            powerup.Consumed();
        }
    }

    public void SpawnRandomPowerup()
    {
        SpawnPowerup(powerupPrefabs[Random.Range(0, powerupPrefabs.Length)]);
    }

    public bool SpawnPowerup(GameObject powerupPrefab, Point? position = null)
    {
        //Find spawn location
        if(position.HasValue && !PowerupCanSpawn(position.Value)) { return false; }
        var spawn = position.HasValue ? position.Value : PickRandomSpawn();
        var tries = 0;
        while (!PowerupCanSpawn(spawn) && tries < MaxPowerupSpawnTries) { spawn = PickRandomSpawn(); ++tries; }
        if(tries >= MaxPowerupSpawnTries) { return false; }
        //Spawn powerup
        var powerup = Instantiate(powerupPrefab).GetComponent<Powerup>();
        powerup.transform.parent = powerupContainer.transform;
        powerup.Position = spawn;
        powerups.Add(powerup);
        return true;
    }

    private Point PickRandomSpawn() { return new Point(Random.Range(0, width), Random.Range(0, height)); }

    /// <summary>Determines, if a powerup can spawn at the given location.</summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool PowerupCanSpawn(Point position)
    {
        return (walls == null || !walls[position.Y, position.X]) &&
            !players.SelectMany(p => p.BodyPositions).Any(pos => pos.X == position.X && pos.Y == position.Y) &&
            position.X >= 0 && position.Y >= 0 &&
            position.X < width && position.Y < height;
    }
}
