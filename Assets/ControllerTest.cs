using UnityEngine;
using System.Collections;

public class ControllerTest : MonoBehaviour
{
    public GameObject grid;
    void Start()
    {
        var field = Instantiate(grid);
        field.GetComponent<Grid>().SetWallLayout(WallLayouts.Border.CreateArray(15, 15));
    }
}
