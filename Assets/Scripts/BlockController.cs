using UnityEngine;
using System.Collections;

/// <summary>
/// ブロックを配置できるクラス
/// </summary>
public class BlockController : MonoBehaviour
{


    public int chip_num_x;
    public int chip_num_y;

    public float chip_size;

    public GameObject[,] blocks;



    void Start()
    {
        chip_num_x = 10;
        chip_num_y = 10;
        chip_size = 1.0f;

        blocks = new GameObject[chip_num_y, chip_num_x];


        for (int y = 0; y < chip_num_y; y++)
        {
            for (int x = 0; x < chip_num_x; x++)
            {
                GameObject block;
                block = Resources.Load<GameObject>("Prefabs/BlockBase");

                block.transform.position = new Vector2(chip_size * x, chip_size * y * -1);
                block.transform.localScale = new Vector2(chip_size, chip_size);

                blocks[y, x] = Instantiate(block);
                //blocks[y, x].transform.parent = gameObject.transform;
                //block.transform.parent = gameObject.transform;
            }
        }

    }

    GameObject getClickObj()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var colition = Physics2D.OverlapPoint(pos);

            if (colition)
            {
                var hitobj = Physics2D.Raycast(pos, -Vector2.up);
                if (hitobj)
                {
                    Debug.Log(hitobj.collider.gameObject.name);
                    return hitobj.collider.gameObject;
                }
            }
        }
        return null;
    }

    void Update()
    {

    }
}
