using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour
{

    [SerializeField]
    Grid grid;
    void Start()
    {
        is_grid = true;
    }

    bool is_grid;
    public void gridChange()
    {
        is_grid = !is_grid;
    }

    void Update()
    {
        if (is_grid)
        {
            grid.gameObject.SetActive(true);
        }
        else
        {
            grid.gameObject.SetActive(false);
        }
    }
}
