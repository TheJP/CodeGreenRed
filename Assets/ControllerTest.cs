using UnityEngine;
using System.Collections;

public class ControllerTest : MonoBehaviour
{
    public GameObject gridPrefab;
    public GameObject playerPrefab;

    private Player player;
    void Start()
    {
        var grid = Instantiate(gridPrefab).GetComponent<Grid>();
        grid.SetWallLayout(WallLayouts.Border.CreateArray(grid.width, grid.height));
        player = Instantiate(playerPrefab).GetComponent<Player>();
        player.SetInitialPosition(new Point(5, 5), Directions.East);
        player.transform.parent = grid.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) { player.Move(); }
        if (Input.GetKeyDown(KeyCode.A)) { player.TurnLeft(); }
        if (Input.GetKeyDown(KeyCode.D)) { player.TurnRight(); }
        if (Input.GetKeyDown(KeyCode.G)) { player.Grow(); }
    }
}
