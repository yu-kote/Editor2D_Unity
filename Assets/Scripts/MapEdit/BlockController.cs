using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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


    // ブロックを増やしたときに分かりにくいのでfloorだけ初期値としてサンプルのspriteを入れておく
    public List<List<List<GameObject>>> blocks;


    [SerializeField]
    UIController uicontroller;

    [SerializeField]
    EnemyEditController enemyeditcontroller;

    [SerializeField]
    Grid grid = null;

    Sprite[] sprites;


    void Awake()
    {
        var grid_width = grid.grid_width / 2;
        chip_start_pos = new Vector2(grid_width, -grid_width);

        chip_interval = 1.0f;

        sprites = Resources.LoadAll<Sprite>("Textures/Floor");

        blocks = new List<List<List<GameObject>>>();
        List<GameObject> tempblock_x = new List<GameObject>();
        List<List<GameObject>> tempblock_xy = new List<List<GameObject>>();


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
                                    "Floor_0"));

                        {
                            var renderer_rect = renderer.sprite.rect;
                            var size = (int)(renderer_rect.width);
                            size = size / 16;

                            chip_size = 6.0f / size;
                        }
                        block.GetComponent<BlockStatus>().number = 0;
                    }
                    else
                    {
                        var renderer = block.GetComponent<SpriteRenderer>();
                        renderer.sprite = null;
                        block.GetComponent<BlockStatus>().number = -1;
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
                                         "Floor_0"));
                    {
                        var renderer_rect = renderer.sprite.rect;
                        var size = (int)(renderer_rect.width);
                        size = size / 16;

                        chip_size = 6.0f / size;
                    }

                    block.GetComponent<BlockStatus>().number = 0;
                }
                else
                {
                    var renderer = block.GetComponent<SpriteRenderer>();
                    renderer.sprite = null;
                    block.GetComponent<BlockStatus>().number = -1;
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
                                      "Floor_0"));
                    {
                        var renderer_rect = renderer.sprite.rect;
                        var size = (int)(renderer_rect.width);
                        size = size / 16;

                        chip_size = 6.0f / size;
                    }

                    block.GetComponent<BlockStatus>().number = 0;
                }
                else
                {
                    var renderer = block.GetComponent<SpriteRenderer>();
                    renderer.sprite = null;
                    block.GetComponent<BlockStatus>().number = -1;
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

    /// <summary>
    /// データ書き出し関数
    /// </summary>
    /// <param name="savename_">txtに書き出すときの名前</param>
    public void saveFunc(string savename_)
    {
        StreamWriter status_sw = new StreamWriter("Assets/SaveFile/" + savename_ + "_StatusData.txt", false);

        string status_writeline = chip_num_x.ToString() + " " + chip_num_y.ToString() + " ";
        status_sw.WriteLine(status_writeline);
        status_sw.Flush();
        status_sw.Close();

        StreamWriter floor_sw = new StreamWriter("Assets/SaveFile/" + savename_ + "_FloorData.txt", false);
        StreamWriter wall_sw = new StreamWriter("Assets/SaveFile/" + savename_ + "_WallData.txt", false);
        StreamWriter door_sw = new StreamWriter("Assets/SaveFile/" + savename_ + "_DoorData.txt", false);
        StreamWriter object_sw = new StreamWriter("Assets/SaveFile/" + savename_ + "_ObjectData.txt", false);
        StreamWriter event_sw = new StreamWriter("Assets/SaveFile/" + savename_ + "_EventData.txt", false);

        string writeline = null;
        for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
        {
            for (int y = 0; y < chip_num_y; y++)
            {
                for (int x = 0; x < chip_num_x; x++)
                {
                    writeline += blocks[i][y][x].GetComponent<BlockStatus>().number.ToString() + " ";
                }

                switch (i)
                {
                    case (int)UIController.SelectLayer.FLOOR:
                        floor_sw.WriteLine(writeline);
                        break;
                    case (int)UIController.SelectLayer.WALL:
                        wall_sw.WriteLine(writeline);
                        break;
                    case (int)UIController.SelectLayer.DOOR:
                        door_sw.WriteLine(writeline);
                        break;
                    case (int)UIController.SelectLayer.OBJECT:
                        object_sw.WriteLine(writeline);
                        break;
                    case (int)UIController.SelectLayer.EVENT:
                        event_sw.WriteLine(writeline);
                        break;
                }
                writeline = null;
            }
            switch (i)
            {
                case (int)UIController.SelectLayer.FLOOR:
                    floor_sw.Flush();
                    floor_sw.Close();
                    break;
                case (int)UIController.SelectLayer.WALL:
                    wall_sw.Flush();
                    wall_sw.Close();
                    break;
                case (int)UIController.SelectLayer.DOOR:
                    door_sw.Flush();
                    door_sw.Close();
                    break;
                case (int)UIController.SelectLayer.OBJECT:
                    object_sw.Flush();
                    object_sw.Close();
                    break;
                case (int)UIController.SelectLayer.EVENT:
                    event_sw.Flush();
                    event_sw.Close();
                    break;
            }
        }

        enemyeditcontroller.save(savename_);
    }


    public void loadFunc(string loadname_)
    {
        for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
        {
            for (int y = 0; y < chip_num_y; y++)
            {
                for (int x = 0; x < chip_num_x; x++)
                {
                    Destroy(blocks[i][y][x]);
                }
            }
        }
        blocks.Clear();

        using (StreamReader sr = new StreamReader("Assets/SaveFile/" + loadname_ + "_StatusData.txt"))
        {
            string line = sr.ReadLine();
            chip_num_x = stringToInt(line, 0);
            chip_num_y = stringToInt(line, 1);
        }


        for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
        {
            string layername = uicontroller.layerToString(i);
            string spritename = layername;
            if (i == (int)UIController.SelectLayer.ENEMY)
            {
                layername = "Enemy0";
            }
            Sprite[] loadsprite = Resources.LoadAll<Sprite>("Textures/" + spritename);
            using (StreamReader sr = new StreamReader("Assets/SaveFile/" + loadname_ + "_" + layername + "Data.txt"))
            {

                List<List<GameObject>> tempblock_xy = new List<List<GameObject>>();
                for (int y = 0; y < chip_num_y; y++)
                {
                    string line = sr.ReadLine();

                    List<GameObject> tempblock_x = new List<GameObject>();
                    for (int x = 0; x < chip_num_x; x++)
                    {
                        GameObject block;
                        block = Resources.Load<GameObject>("Prefabs/BlockBase");

                        int number = stringToInt(line, x);

                        var renderer = block.GetComponent<SpriteRenderer>();
                        if (number != -1)
                        {
                            renderer.sprite =
                                System.Array.Find<Sprite>(
                                    loadsprite, (sprite) => sprite.name.Equals(
                                        spritename + "_" + number.ToString()));

                            {
                                var renderer_rect = renderer.sprite.rect;
                                var size = (int)(renderer_rect.width);
                                size = size / 16;

                                chip_size = 6.0f / size;
                            }
                        }
                        else
                        {
                            renderer.sprite = null;
                        }

                        block.GetComponent<BlockStatus>().number = number;

                        block.transform.position = new Vector3(chip_interval * x, chip_interval * y * -1, -i) + new Vector3(chip_start_pos.x, chip_start_pos.y, 0);
                        block.transform.localScale = new Vector2(chip_size, chip_size);

                        tempblock_x.Add(Instantiate(block));
                        tempblock_x[x].transform.parent = gameObject.transform;
                    }
                    tempblock_xy.Add(tempblock_x);
                }
                blocks.Add(tempblock_xy);
            }
        }
        enemyeditcontroller.load(loadname_);

    }


    /// <summary>
    /// 文字列を空白区切りでintに変換する関数
    /// </summary>
    /// <param name="value_"></param>
    /// <param name="num_"></param>
    /// <returns></returns>
    public int stringToInt(string value_, int num_)
    {
        int retvalue = 0;
        int value_count = 0;
        string temp_value = "";
        char[] c = value_.ToCharArray();
        bool is_blank = false;

        //3 3 0 0 
        for (int i = 0; i < value_.Length; i++)
        {
            if (is_blank)
            {
                if (c[i] != ' ')
                {
                    is_blank = false;
                }
            }
            if (is_blank == false)
            {
                if (c[i] == ' ')
                {
                    is_blank = true;

                    if (value_count == num_)
                    {
                        retvalue = int.Parse(temp_value);
                        break;
                    }
                    temp_value = "";
                    value_count++;
                    continue;
                }
                temp_value += c[i];
            }

        }
        return retvalue;
    }
}
