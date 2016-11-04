using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ブロックの管理とブロックを配置できる範囲を決めるクラス
/// </summary>
public class BlockController : MonoBehaviour
{

    public Vector2 chip_start_pos;
    [SerializeField]
    public int chip_num_x;
    [SerializeField]
    public int chip_num_y;

    public float chip_size;
    public float chip_interval;

    // レイヤーわけのために4つ二重配列を作る 
    // FIXME: レイヤーが増えた場合に対応できていない。

    // ブロックを増やしたときに分かりにくいのでfloorだけ初期値として床のspriteを入れておく
    public List<List<List<GameObject>>> blocks;

    [SerializeField]
    UIController uicontroller;

    [SerializeField]
    Grid grid = null;

    Sprite[] sprites;


    void Start()
    {
        var grid_width = grid.grid_width / 2;
        chip_start_pos = new Vector2(grid_width, -grid_width);

        chip_size = 6.0f;
        chip_interval = 1.0f;

        sprites = Resources.LoadAll<Sprite>("Textures/samplechip");

        blocks = new List<List<List<GameObject>>>();
        List<GameObject> tempblock_x = new List<GameObject>();
        List<List<GameObject>> tempblock_xy = new List<List<GameObject>>();
        int texture_num = 7;


        // 初期ブロックを配置
        for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
        {
            for (int y = 0; y < chip_num_y; y++)
            {
                for (int x = 0; x < chip_num_x; x++)
                {
                    GameObject block;
                    block = Resources.Load<GameObject>("Prefabs/BlockBase");
                    if (i == (int)UIController.SelectLayer.FLOOR)
                    {
                        var renderer = block.GetComponent<SpriteRenderer>();
                        renderer.sprite =
                            System.Array.Find<Sprite>(
                                sprites, (sprite) => sprite.name.Equals(
                                    "samplechip_" + texture_num.ToString()));

                    }
                    else
                    {
                        var renderer = block.GetComponent<SpriteRenderer>();
                        renderer.sprite = null;
                    }
                    block.transform.position = new Vector3(chip_interval * x, chip_interval * y * -1, -i) + new Vector3(chip_start_pos.x, chip_start_pos.y, 0);

                    block.transform.localScale = new Vector2(chip_size, chip_size);

                    tempblock_x.Add(Instantiate(block));
                    tempblock_x[x].transform.parent = gameObject.transform;
                }
                tempblock_xy.Add(tempblock_x);
                tempblock_x = new List<GameObject>();
            }
            blocks.Add(tempblock_xy);
            tempblock_xy = new List<List<GameObject>>();
        }
    }

    /// <summary>
    /// Cellの X 要素を足す
    /// </summary>
    void addToCellX()
    {
        List<GameObject> tempblock_y = new List<GameObject>();
        for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
        {
            for (int y = 0; y < chip_num_y; y++)
            {
                GameObject block;
                block = Resources.Load<GameObject>("Prefabs/BlockBase");
                if (i == (int)UIController.SelectLayer.FLOOR)
                {
                    var renderer = block.GetComponent<SpriteRenderer>();
                    renderer.sprite =
                        System.Array.Find<Sprite>(
                            sprites, (sprite) => sprite.name.Equals(
                                "samplechip_" + 7.ToString()));
                }
                else
                {
                    var renderer = block.GetComponent<SpriteRenderer>();
                    renderer.sprite = null;
                }

                block.transform.position = new Vector3(chip_interval * chip_num_x, chip_interval * y * -1, -i) + new Vector3(chip_start_pos.x, chip_start_pos.y, 0);

                block.transform.localScale = new Vector2(chip_size, chip_size);

                blocks[i][y].Add(Instantiate(block));
                blocks[i][y][chip_num_x].transform.parent = gameObject.transform;
            }
        }
        chip_num_x += 1;
    }
    /// <summary>
    /// Cellの Y 要素を足す
    /// </summary>
    void addToCellY()
    {
        List<GameObject> tempblock_x = new List<GameObject>();
        for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
        {
            for (int x = 0; x < chip_num_x; x++)
            {
                GameObject block;
                block = Resources.Load<GameObject>("Prefabs/BlockBase");
                if (i == (int)UIController.SelectLayer.FLOOR)
                {
                    var renderer = block.GetComponent<SpriteRenderer>();
                    renderer.sprite =
                        System.Array.Find<Sprite>(
                            sprites, (sprite) => sprite.name.Equals(
                                "samplechip_" + 7.ToString()));
                }
                else
                {
                    var renderer = block.GetComponent<SpriteRenderer>();
                    renderer.sprite = null;
                }

                block.transform.position = new Vector3(chip_interval * x, chip_interval * chip_num_y * -1, -i) + new Vector3(chip_start_pos.x, chip_start_pos.y, 0);

                block.transform.localScale = new Vector2(chip_size, chip_size);

                tempblock_x.Add(Instantiate(block));
                tempblock_x[x].transform.parent = gameObject.transform;
            }
            blocks[i].Add(tempblock_x);
            tempblock_x = new List<GameObject>();
        }

        chip_num_y += 1;
    }
    /// <summary>
    /// Cellの X 要素を減らす
    /// </summary>
    void removeToCellX()
    {
        if (chip_num_x <= 1)
            return;
        for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
        {
            for (int y = 0; y < chip_num_y; y++)
            {
                Destroy(blocks[i][y][chip_num_x - 1]);
                blocks[i][y].RemoveAt(chip_num_x - 1);
            }
        }
        chip_num_x -= 1;
    }
    /// <summary>
    /// Cellの Y 要素を減らす
    /// </summary>
    void removeToCellY()
    {
        if (chip_num_y <= 1)
            return;
        for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
        {
            for (int x = 0; x < chip_num_x; x++)
            {
                Destroy(blocks[i][chip_num_y - 1][x]);
            }
            blocks[i].RemoveAt(chip_num_y - 1);
        }
        chip_num_y -= 1;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            addToCellX();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            addToCellY();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            removeToCellX();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            removeToCellY();
        }
    }
}
