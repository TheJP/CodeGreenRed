using UnityEngine;
using System.Collections;

public class ControllerTest : MonoBehaviour
{
    public GameObject grid;
    void Start()
    {
        var field = Instantiate(grid);
        var walls = new bool[15, 15];
        walls[0, 0] = true;
        walls[5, 0] = true;
        walls[5, 5] = true;
        walls[7, 7] = true;
        walls[8, 7] = true;
        field.GetComponent<Grid>().SetWallLayout(walls);
    }
}
