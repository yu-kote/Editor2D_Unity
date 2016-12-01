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

    [SerializeField]
    MapEditController editcontroller;

    // 選択したエネミーの中の、選択したルートの中の、y軸と,x軸
    public List<List<List<List<GameObject>>>> enemylist;

    Sprite[] sprites;

    // エネミーの数
    public int enemylayer_max;
    // エネミーのルートの数
    public int enemyroot_max;


    // 今選んでいるエネミー
    public int selectenemy;
    // 前選んでいたエネミー
    public int prev_selectenemy;

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


    /// <summary>
    /// エネミーのルートを置く処理
    /// </summary>
    public void editEnemyRoot()
    {
        var click_obj = getClickObj();
        if (click_obj == null) return;

        int selectnum = editcontroller.current_select_block_num;

        var renderer = click_obj.GetComponent<SpriteRenderer>();
        renderer.sprite =
            System.Array.Find<Sprite>(
                sprites, (sprite) => sprite.name.Equals(
                    "Enemy_" + selectnum.ToString()));


        var renderer_rect = renderer.sprite.rect;

        var size = new Vector3((int)(renderer_rect.width), (int)(renderer_rect.height), 0);

        size = size / 16;
        var scale = click_obj.transform.localScale;
        scale = new Vector2(6.0f / size.x, 6.0f / size.y);

        click_obj.transform.localScale = scale;

        // ステータスを変える
        click_obj.GetComponent<BlockStatus>().number = selectnum;
    }

    public void enemyLayerChangeSpriteClear()
    {
        selectEnemySpriteClear(selectenemy);
    }
    public void enemyLayerChangeSpriteDraw()
    {
        prev_selectenemy = 1;
        selectenemy = 0;
    }

    /// <summary>
    /// クリックしたブロックを消す
    /// </summary>
    public void removeEnemyRoot()
    {
        var click_obj = getClickObj();
        if (click_obj == null) return;

        click_obj.GetComponent<BlockStatus>().clear();
    }


    /// <summary>
    /// クリックしたら選んでいるエネミーの選んでいるルートのGameObjectを返す関数
    /// </summary>
    public GameObject getClickObj()
    {
        if (Input.GetMouseButton(0))
        {
            if (editcontroller.getMouseHitUI() == null)
            {
                return getHitRootObj(editcontroller.mousePosToCell());
            }
        }
        return null;
    }

    /// <summary>
    /// マウスにあたっている、選んでいるエネミーの選んでいるルートのGameObjectを返す関数
    /// </summary>
    /// <param name="mousecell_">マウスの位置</param>
    /// <returns>ルート一個分のGameObject</returns>
    public GameObject getHitRootObj(Vector2 mousecell_)
    {
        if (mousecell_.x < 0 || mousecell_.y < 0) return null;
        if (mousecell_.x > blockcontroller.chip_num_x - 1 ||
            mousecell_.y > blockcontroller.chip_num_y - 1) return null;

        int rootnum = getSelectblocknumToEnemyrootnum(editcontroller.current_select_block_num);
        return enemylist[selectenemy][rootnum][(int)mousecell_.y][(int)mousecell_.x];
    }

    /// <summary>
    /// 選んでいるブロック番号をエネミーのルートの番号に変換する関数
    /// </summary>
    /// <param name="selectblocknum_"></param>
    /// <returns></returns>
    int getSelectblocknumToEnemyrootnum(int selectblocknum_)
    {
        return selectblocknum_ / 2;
    }

    /// <summary>
    /// 引数でもらったエネミーの全てのルートの画像にnullを入れる(番号は保ったまま)
    /// </summary>
    /// <param name="selectenemy_">画像をクリアするエネミー</param>
    void selectEnemySpriteClear(int selectenemy_)
    {
        for (int k = 0; k < enemyroot_max; k++)
        {
            for (int y = 0; y < blockcontroller.chip_num_y; y++)
            {
                for (int x = 0; x < blockcontroller.chip_num_x; x++)
                {
                    enemylist[selectenemy_][k][y][x].GetComponent<SpriteRenderer>().sprite = null;
                }
            }
        }
    }

    /// <summary>
    /// 引数でもらったエネミーの全てのルートに自分の持っている番号の画像を入れる関数
    /// </summary>
    /// <param name="selectenemy_"></param>
    void selectEnemySpriteDraw(int selectenemy_)
    {
        for (int k = 0; k < enemyroot_max; k++)
        {
            for (int y = 0; y < blockcontroller.chip_num_y; y++)
            {
                for (int x = 0; x < blockcontroller.chip_num_x; x++)
                {
                    int num = enemylist[selectenemy_][k][y][x].GetComponent<BlockStatus>().number;
                    var renderer = enemylist[selectenemy_][k][y][x].GetComponent<SpriteRenderer>();

                    if (num != -1)
                        renderer.sprite = System.Array.Find<Sprite>(
                                    sprites, (sprite) => sprite.name.Equals(
                                        "Enemy_" + num.ToString()));
                }
            }
        }
    }

    /// <summary>
    /// xの要素追加
    /// </summary>
    public void addToCellX()
    {
        for (int i = 0; i < enemylayer_max; i++)
        {
            for (int k = 0; k < enemyroot_max; k++)
            {
                for (int y = 0; y < blockcontroller.chip_num_y; y++)
                {
                    GameObject root = Resources.Load<GameObject>("Prefabs/BlockBase");
                    root.GetComponent<BlockStatus>().clear();

                    root.transform.position =
                        blockcontroller
                        .blocks[(int)UIController.SelectLayer.ENEMY][y][blockcontroller.chip_num_x - 1]
                        .transform.position;

                    enemylist[i][k][y].Add(Instantiate(root));
                    enemylist[i][k][y][blockcontroller.chip_num_x - 1].transform.parent = gameObject.transform;
                }
            }
        }
    }

    /// <summary>
    /// yの要素の追加
    /// </summary>
    public void addToCellY()
    {
        for (int i = 0; i < enemylayer_max; i++)
        {
            for (int k = 0; k < enemyroot_max; k++)
            {
                List<GameObject> tempenemy_x = new List<GameObject>();
                for (int x = 0; x < blockcontroller.chip_num_x; x++)
                {
                    GameObject root = Resources.Load<GameObject>("Prefabs/BlockBase");
                    root.GetComponent<BlockStatus>().clear();

                    root.transform.position =
                        blockcontroller
                        .blocks[(int)UIController.SelectLayer.ENEMY][blockcontroller.chip_num_y - 1][x]
                        .transform.position;


                    tempenemy_x.Add(Instantiate(root));
                    tempenemy_x[x].transform.parent = gameObject.transform;
                }
                enemylist[i][k].Add(tempenemy_x);
            }
        }
    }

    /// <summary>
    /// xの要素を減らす
    /// </summary>
    public void removeToCellX()
    {
        for (int i = 0; i < enemylayer_max; i++)
        {
            for (int k = 0; k < enemyroot_max; k++)
            {
                for (int y = 0; y < blockcontroller.chip_num_y; y++)
                {
                    Destroy(enemylist[i][k][y][blockcontroller.chip_num_x]);
                    enemylist[i][k][y].RemoveAt(blockcontroller.chip_num_x);
                }
            }
        }
    }

    /// <summary>
    /// yの要素を減らす
    /// </summary>
    public void removeToCellY()
    {
        for (int i = 0; i < enemylayer_max; i++)
        {
            for (int k = 0; k < enemyroot_max; k++)
            {
                for (int x = 0; x < blockcontroller.chip_num_x; x++)
                {
                    Destroy(enemylist[i][k][blockcontroller.chip_num_y][x]);
                }
                enemylist[i][k].RemoveAt(blockcontroller.chip_num_y);
            }
        }
    }

    void Start()
    {
        selectenemy = 0;
        prev_selectenemy = selectenemy;
        enemylayer_max = 5;
        enemyroot_max = 10;

        enemylist = new List<List<List<List<GameObject>>>>();


        for (int i = 0; i < enemylayer_max; i++)
        {
            List<List<List<GameObject>>> tempenemyrootlayer = new List<List<List<GameObject>>>();
            for (int k = 0; k < enemyroot_max; k++)
            {
                List<List<GameObject>> tempenemy_xy = new List<List<GameObject>>();
                for (int y = 0; y < blockcontroller.chip_num_y; y++)
                {
                    List<GameObject> tempenemy_x = new List<GameObject>();
                    for (int x = 0; x < blockcontroller.chip_num_x; x++)
                    {

                        GameObject root = Resources.Load<GameObject>("Prefabs/BlockBase");

                        root.GetComponent<BlockStatus>().clear();

                        root.transform.position =
                            blockcontroller.blocks[(int)UIController.SelectLayer.ENEMY][y][x].transform.position;

                        tempenemy_x.Add(Instantiate(root));
                        tempenemy_x[x].transform.parent = gameObject.transform;
                    }
                    tempenemy_xy.Add(tempenemy_x);
                }
                tempenemyrootlayer.Add(tempenemy_xy);
            }
            enemylist.Add(tempenemyrootlayer);
        }

        sprites = Resources.LoadAll<Sprite>("Textures/Enemy");
    }


    void Update()
    {
        if (prev_selectenemy == selectenemy) return;
        selectEnemySpriteClear(prev_selectenemy);
        selectEnemySpriteDraw(selectenemy);
        prev_selectenemy = selectenemy;
    }

    bool enemyRootCheck(int enemylayer_, int enemyroot_)
    {
        for (int y = 0; y < blockcontroller.chip_num_y; y++)
        {
            for (int x = 0; x < blockcontroller.chip_num_x; x++)
            {
                if (enemylist[enemylayer_][enemyroot_][y][x].GetComponent<BlockStatus>().number != -1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void save(string savename_)
    {

        for (int i = 0; i < enemylayer_max; i++)
        {
            int count = 0;
            for (int k = 0; k < enemyroot_max; k++)
            {
                if (enemyRootCheck(i, k) == false) continue;

                StreamWriter sw = new StreamWriter
                    ("Assets/SaveFile/" + savename_
                    + "_Enemy" + i.ToString()
                    + "Root" + count.ToString()
                    + "Data.txt", false);
                count++;
                for (int y = 0; y < blockcontroller.chip_num_y; y++)
                {
                    string writeline = null;

                    for (int x = 0; x < blockcontroller.chip_num_x; x++)
                    {
                        writeline += enemylist[i][k][y][x].GetComponent<BlockStatus>().number.ToString() + " ";
                    }
                    sw.WriteLine(writeline);
                }
                sw.Flush();
                sw.Close();
            }

        }
    }

    public void allClear()
    {
        for (int i = 0; i < enemylayer_max; i++)
        {
            for (int k = 0; k < enemyroot_max; k++)
            {
                for (int y = 0; y < blockcontroller.chip_num_y; y++)
                {
                    for (int x = 0; x < blockcontroller.chip_num_x; x++)
                    {
                        Destroy(enemylist[i][k][y][x]);
                    }
                }
            }
        }
        enemylist.Clear();
    }

    public void load(string loadname_)
    {
        for (int i = 0; i < enemylayer_max; i++)
        {
            List<List<List<GameObject>>> tempenemyrootlayer = new List<List<List<GameObject>>>();
            for (int k = 0; k < enemyroot_max; k++)
            {
                string readfile = "Assets/SaveFile/" + loadname_
                    + "_Enemy" + i.ToString()
                    + "Root" + k.ToString()
                    + "Data.txt";

                if (!File.Exists(readfile))
                {
                    List<List<GameObject>> nullenemy_xy = new List<List<GameObject>>();
                    for (int y = 0; y < blockcontroller.chip_num_y; y++)
                    {
                        List<GameObject> nullenemy_x = new List<GameObject>();
                        for (int x = 0; x < blockcontroller.chip_num_x; x++)
                        {
                            GameObject root = Resources.Load<GameObject>("Prefabs/BlockBase");

                            root.GetComponent<BlockStatus>().clear();

                            root.transform.position =
                          blockcontroller.blocks[(int)UIController.SelectLayer.ENEMY][y][x].transform.position;
                            root.transform.localScale = new Vector2(6, 6);

                            nullenemy_x.Add(Instantiate(root));
                            nullenemy_x[x].transform.parent = gameObject.transform;
                        }
                        nullenemy_xy.Add(nullenemy_x);
                    }
                    tempenemyrootlayer.Add(nullenemy_xy);
                    continue;
                }
                StreamReader sr = new StreamReader(readfile);

                List<List<GameObject>> tempenemy_xy = new List<List<GameObject>>();
                for (int y = 0; y < blockcontroller.chip_num_y; y++)
                {
                    string line = sr.ReadLine();

                    List<GameObject> tempenemy_x = new List<GameObject>();
                    for (int x = 0; x < blockcontroller.chip_num_x; x++)
                    {
                        GameObject root = Resources.Load<GameObject>("Prefabs/BlockBase");

                        root.GetComponent<BlockStatus>().clear();

                        int number = blockcontroller.stringToInt(line, x);
                        root.GetComponent<BlockStatus>().number = number;

                        root.transform.position = blockcontroller.blocks[(int)UIController.SelectLayer.ENEMY][y][x].transform.position;
                        root.transform.localScale = new Vector2(6, 6);

                        tempenemy_x.Add(Instantiate(root));
                        tempenemy_x[x].transform.parent = gameObject.transform;
                    }
                    tempenemy_xy.Add(tempenemy_x);
                }
                tempenemyrootlayer.Add(tempenemy_xy);
            }
            enemylist.Add(tempenemyrootlayer);
        }

        prev_selectenemy = 1;
        selectenemy = 0;
    }
}

