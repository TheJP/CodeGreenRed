using UnityEngine;
using System.Collections;

public class ControllerTest : MonoBehaviour
{
    public GameObject gridPrefab;

    private Player player;
    private Grid grid;
    void Start()
    {
        grid = Instantiate(gridPrefab).GetComponent<Grid>();
        grid.SetWallLayout(WallLayouts.Border.CreateArray(grid.width, grid.height));
        player = grid.AddPlayer(new Point(5, 5), Directions.North, Teams.Red);
        grid.AddPlayer(new Point(10, 5), Directions.North, Teams.Red);
        grid.AddPlayer(new Point(5, 10), Directions.South, Teams.Green);
        grid.AddPlayer(new Point(10, 10), Directions.South, Teams.Green);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) { player.Move(); }
        if (Input.GetKeyDown(KeyCode.A)) { player.TurnLeft(); }
        if (Input.GetKeyDown(KeyCode.D)) { player.TurnRight(); }
        if (Input.GetKeyDown(KeyCode.G)) { player.Grow(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { player.Move(2); }
        if (Input.GetKeyDown(KeyCode.U)) { grid.SpawnRandomPowerup(); }
    }
}
