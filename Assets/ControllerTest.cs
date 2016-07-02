using UnityEngine;
using System.Collections;

public class ControllerTest : MonoBehaviour
{
    public GameObject grid;
    void Start()
    {
        var field = Instantiate(grid).GetComponent<Grid>();
        field.SetWallLayout(WallLayouts.Border.CreateArray(field.width, field.height));
    }
}
