using UnityEngine;
using System.Collections;

/// <summary>
/// ブロックを配置するクラス
/// </summary>
public class BlockController : MonoBehaviour
{

    public Vector2 chip_start_pos;
    public int chip_num_x;
    public int chip_num_y;

    public float chip_size;
    public float chip_interval;

    public GameObject[,] blocks;

    [SerializeField]
    Grid grid = null;


    void Start()
    {
        var grid_width = grid.grid_width / 2;
        chip_start_pos = new Vector2(grid_width, -grid_width);
        chip_num_x = 10;
        chip_num_y = 10;
        chip_size = 6.0f;
        chip_interval = 1.0f;

        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/samplechip");

        blocks = new GameObject[chip_num_y, chip_num_x];


        int texture_num = 7;



        for (int y = 0; y < chip_num_y; y++)
        {
            for (int x = 0; x < chip_num_x; x++)
            {
                GameObject block;
                block = Resources.Load<GameObject>("Prefabs/BlockBase");
                var renderer = block.GetComponent<SpriteRenderer>();
                renderer.sprite =
                    System.Array.Find<Sprite>(
                        sprites, (sprite) => sprite.name.Equals(
                            "samplechip_" + texture_num.ToString()));


                block.transform.position = new Vector2(chip_interval * x, chip_interval * y * -1) + chip_start_pos;

                block.transform.localScale = new Vector2(chip_size, chip_size);

                blocks[y, x] = Instantiate(block);
                blocks[y, x].transform.parent = gameObject.transform;
            }
        }
    }

    Vector3 testmousepos;
    GameObject getClickObj()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseposition = Input.mousePosition;
            //mouseposition.z -= 10.0f;
            var screen_to_world_point_position = Camera.main.ScreenToWorldPoint(mouseposition);

            testmousepos = screen_to_world_point_position;
            var colition = Physics2D.OverlapPoint(screen_to_world_point_position);

            if (colition != null)
            {
                //var hitobj = Physics2D.Raycast(screen_to_world_point_position, -Vector2.up);
                var collider = colition.transform.gameObject;
                Debug.Log(collider.name);
                return collider;
            }
        }
        return null;
    }


    void Update()
    {
        var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        mousepos += new Vector3(chip_start_pos.x, chip_start_pos.y, 0);
        var mousecell_x = Mathf.FloorToInt(mousepos.x);
        var mousecell_y = Mathf.FloorToInt(mousepos.y) * -1 - 1;
        if (mousecell_x < 0 || mousecell_y < 0) return;
        if (mousecell_x > chip_num_x - 1 || mousecell_y > chip_num_y - 1) return;


        blocks[mousecell_y, mousecell_x].GetComponent<SpriteRenderer>().material.color = Color.red;
    }
}
