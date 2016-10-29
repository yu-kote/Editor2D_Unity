using UnityEngine;
using System.Collections;

/// <summary>
/// グリッド線を引くクラス
/// </summary>
public class Grid : MonoBehaviour
{
    int grid_num = 100;
    Vector2 grid_startpos;
    Vector2[] grid_start = null;
    Vector2[] grid_end = null;

    float grid_length = 100;
    float grid_interval = 1;

    void Start()
    {
        grid_startpos = new Vector2(-50, 50);
        grid_start = new Vector2[grid_num];
        grid_end = new Vector2[grid_num];

        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();

        //transform.parent = grids.transform;
        for (int x = 0; x < grid_num; x++)
        {
            GameObject grids = new GameObject();
            grids.AddComponent<LineRenderer>();

            var lr = grids.GetComponent<LineRenderer>();
            grids.transform.parent = gameObject.transform;

            lr.SetWidth(0.05f, 0.05f);
            lr.SetVertexCount(2);

            grid_start[x].x = grid_length + grid_startpos.x;
            grid_start[x].y = grid_interval * -x + grid_startpos.y;
            grid_end[x].x = grid_length * -1 + grid_startpos.x;
            grid_end[x].y = grid_interval * -x + grid_startpos.y;

            lr.SetPosition(0, new Vector2(grid_start[x].x, grid_start[x].y));
            lr.SetPosition(1, new Vector2(grid_end[x].x, grid_end[x].y));
        }
        for (int y = 0; y < grid_num; y++)
        {
            GameObject grids = new GameObject();
            grids.AddComponent<LineRenderer>();

            var lr = grids.GetComponent<LineRenderer>();
            grids.transform.parent = gameObject.transform;

            lr.SetWidth(0.05f, 0.05f);
            lr.SetVertexCount(2);

            grid_start[y].x = grid_interval * y + grid_startpos.x;
            grid_start[y].y = grid_length + grid_startpos.y;
            grid_end[y].x = grid_interval * y + grid_startpos.x;
            grid_end[y].y = grid_length * -1 + grid_startpos.y;

            lr.SetPosition(0, new Vector2(grid_start[y].x, grid_start[y].y));
            lr.SetPosition(1, new Vector2(grid_end[y].x, grid_end[y].y));
        }


    }


    void Update()
    {

    }
}
