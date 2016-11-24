using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// エネミーのレイヤーだけ特別な仕様なので、別途で管理クラスを作る
/// </summary>
public class EnemyEditController : MonoBehaviour
{
    [SerializeField]
    BlockController blockcontroller;

    public int[][][] enemylist;
    Sprite[] sprites;

    // エネミーの数
    public int enemylayer_max;

    // 今選んでいるエネミー
    public int selectenemy;
    public int current_selectenemy;

    // エネミーボタンが押されたときにレイヤーを変更する関数たち
    public void enemy0Change()
    {
        selectenemy = 0;
    }
    public void enemy1Change()
    {
        selectenemy = 1;
    }
    public void enemy2Change()
    {
        selectenemy = 2;
    }
    public void enemy3Change()
    {
        selectenemy = 3;
    }
    public void enemy4Change()
    {
        selectenemy = 4;
    }

    public void enemyListUpdate(int layer_)
    {
        int i = (int)UIController.SelectLayer.ENEMY;
        //エネミーのレイヤーを消す
        if (enemylist[layer_].Length > 0)
        {
            for (int y = 0; y < enemylist[layer_].Length; y++)
            {
                for (int x = 0; x < enemylist[layer_][y].Length; x++)
                {
                    enemylist[layer_][y][x] = -1;
                }
            }
        }


        // いまのマップ情報を消したところに保存する
        int[][] temp_xy = new int[blockcontroller.chip_num_y][];
        for (int y = 0; y < blockcontroller.chip_num_y; y++)
        {
            int[] temp_x = new int[blockcontroller.chip_num_x];

            for (int x = 0; x < blockcontroller.chip_num_x; x++)
            {
                temp_x[x] = blockcontroller.blocks[i][y][x].GetComponent<BlockStatus>().number;

            }
            temp_xy[y] = temp_x;
        }
        enemylist[layer_] = temp_xy;
    }

    public void setEnemyLayer()
    {
        int i = (int)UIController.SelectLayer.ENEMY;

        enemyListUpdate(current_selectenemy);

        //今のマップ情報を消す
        for (int y = 0; y < blockcontroller.chip_num_y; y++)
        {
            for (int x = 0; x < blockcontroller.chip_num_x; x++)
            {
                blockcontroller.blocks[i][y][x].GetComponent<BlockStatus>().clear();
            }
        }

        int chip_x;
        int chip_y;
        if (blockcontroller.chip_num_y < enemylist[selectenemy].Length)
            chip_y = blockcontroller.chip_num_y;
        else
            chip_y = enemylist[selectenemy].Length;
        if (blockcontroller.chip_num_x < enemylist[selectenemy][0].Length)
            chip_x = blockcontroller.chip_num_x;
        else
            chip_x = enemylist[selectenemy][0].Length;

        // 新しいレイヤーの情報を今のマップに入れる
        for (int y = 0; y < chip_y; y++)
        {
            for (int x = 0; x < chip_x; x++)
            {
                var block = blockcontroller.blocks[i][y][x];
                var renderer = block.GetComponent<SpriteRenderer>();

                int num = enemylist[selectenemy][y][x];

                if (num != -1)
                {
                    renderer.sprite = System.Array.Find<Sprite>(
                                                       sprites, (sprite) => sprite.name.Equals(
                                                       "Enemy" + "_" +
                                                       num.ToString()));

                    var renderer_rect = renderer.sprite.rect;

                    var size = new Vector3((int)(renderer_rect.width), (int)(renderer_rect.height), 0);

                    size = size / 16;
                    var scale = block.transform.localScale;
                    scale = new Vector2(6.0f / size.x, 6.0f / size.y);
                    block.transform.localScale = scale;
                }
                else
                {
                    renderer.sprite = null;
                }

                block.GetComponent<BlockStatus>().number = num;

            }
        }
    }
    void enemyLayerAllNull()
    {
        for (int i = 0; i < enemylayer_max; i++)
        {
            for (int y = 0; y < enemylist[i].Length; y++)
            {
                for (int x = 0; x < enemylist[i][y].Length; x++)
                {
                    enemylist[i][y][x] = -1;
                }
            }
        }
    }

    void Start()
    {
        selectenemy = 0;
        current_selectenemy = selectenemy;
        enemylayer_max = 5;

        enemylist = new int[enemylayer_max][][];
        for (int i = 0; i < enemylayer_max; i++)
        {
            int[][] temp_x = new int[blockcontroller.chip_num_y][];
            for (int y = 0; y < blockcontroller.chip_num_y; y++)
            {
                temp_x[y] = new int[blockcontroller.chip_num_x];
            }
            enemylist[i] = temp_x;
        }
        enemyLayerAllNull();

        sprites = Resources.LoadAll<Sprite>("Textures/Enemy");
    }


    void Update()
    {
        if (current_selectenemy == selectenemy) return;
        setEnemyLayer();
        current_selectenemy = selectenemy;
    }



    public void save(string savename_)
    {
        enemyListUpdate(selectenemy);
        for (int i = 0; i < enemylayer_max; i++)
        {
            StreamWriter sw = new StreamWriter("Assets/SaveFile/" + savename_ + "_Enemy" + i.ToString() + "Data.txt", false);

            // 配列の長さを合わせる(セーブする要素数が少ない場合、少なく回るように)
            int chip_x;
            int chip_y;
            if (blockcontroller.chip_num_y < enemylist[i].Length)
                chip_y = blockcontroller.chip_num_y;
            else
                chip_y = enemylist[selectenemy].Length;
            if (blockcontroller.chip_num_x < enemylist[i][0].Length)
                chip_x = blockcontroller.chip_num_x;
            else
                chip_x = enemylist[selectenemy][0].Length;

            // セーブする配列を用意
            int[][] temp = new int[blockcontroller.chip_num_y][];
            // 一度初期化する
            for (int y = 0; y < blockcontroller.chip_num_y; y++)
            {
                temp[y] = new int[blockcontroller.chip_num_x];
                for (int x = 0; x < blockcontroller.chip_num_x; x++)
                {
                    temp[y][x] = -1;
                }
            }

            // レイヤー情報をセーブする配列に入れる
            for (int y = 0; y < chip_y; y++)
            {
                for (int x = 0; x < chip_x; x++)
                {
                    temp[y][x] = enemylist[i][y][x];
                }
            }

            string writeline = null;
            for (int y = 0; y < blockcontroller.chip_num_y; y++)
            {
                for (int x = 0; x < blockcontroller.chip_num_x; x++)
                {
                    writeline += temp[y][x].ToString() + " ";
                }
                sw.WriteLine(writeline);
                writeline = null;
            }
            sw.Flush();
            sw.Close();
        }
    }


    public void load(string loadname_)
    {
        selectenemy = 0;
        current_selectenemy = 0;
        for (int i = 0; i < enemylayer_max; i++)
        {
            using (StreamReader sr = new StreamReader("Assets/SaveFile/" + loadname_ + "_" + "Enemy" + i + "Data.txt"))
            {
                int[][] temp = new int[blockcontroller.chip_num_y][];
                for (int y = 0; y < blockcontroller.chip_num_y; y++)
                {
                    temp[y] = new int[blockcontroller.chip_num_x];

                    string line = sr.ReadLine();

                    for (int x = 0; x < blockcontroller.chip_num_x; x++)
                    {
                        int num = blockcontroller.stringToInt(line, x);
                        temp[y][x] = num;
                    }
                }

                enemylist[i] = temp;
            }
        }
    }
}
