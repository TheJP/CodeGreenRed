using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    public GameObject rectangle;
    public int width;
    public int height;
    void Start()
    {
        //Create grid (rectangles)
        for(int y = 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x)
            {
                var tile = (GameObject)Instantiate(rectangle, new Vector3(x - 0.05f, y - 0.05f), Quaternion.identity);
                tile.transform.parent = transform;
            }
        }
    }
}
