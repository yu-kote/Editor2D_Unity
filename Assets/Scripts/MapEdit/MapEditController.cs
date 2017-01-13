using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

/// <summary>
/// マップをエディットするクラス
/// </summary>
public class MapEditController : MonoBehaviour
{
    [SerializeField]
    UIController uicontroller;

    [SerializeField]
    BlockController blockcontroller;

    [SerializeField]
    EnemyEditController enemyeditcontroller;


    // 今選んでいるブロック番号
    public int current_select_block_num;

    // 選択しているレイヤーのspriteたち
    Sprite[] sprites;
    string current_layername;

    public enum EditMode
    {
        WRITE,
        REMOVE,
        CLEAR,
        EDIT_MAX,
    }
    public EditMode editmode;

    private bool is_range_mode;
    private int rangemode_change_time;
    void Start()
    {
        current_select_block_num = 0;
        editmode = EditMode.WRITE;
        current_layername = null;
        is_range_mode = false;
        is_click = false;
        rangemode_change_time = 10;
    }

    void Update()
    {
        changeLayerLoadSprites();

        if (uicontroller.selectlayer == UIController.SelectLayer.ENEMY)
        {
            switch (editmode)
            {
                case EditMode.WRITE:
                    enemyeditcontroller.editEnemyRoot();
                    break;
                case EditMode.REMOVE:
                    enemyeditcontroller.removeEnemyRoot();
                    break;
                case EditMode.CLEAR:
                    enemyeditcontroller.removeEnemyRoot();
                    break;
            }

            return;
        }

        if (is_range_mode == false)
        {
            switch (editmode)
            {
                case EditMode.WRITE:
                    editMap();
                    break;
                case EditMode.REMOVE:
                    removeBlock();
                    break;
                case EditMode.CLEAR:
                    clearBlock();
                    break;
            }
        }
        if (is_range_mode == true)
        {
            rangeDecide();
            if (Input.GetMouseButtonUp(0))
                switch (editmode)
                {
                    case EditMode.WRITE:
                        rangeEditMap();
                        break;
                    case EditMode.REMOVE:
                        rangeRemoveBlock();
                        break;
                    case EditMode.CLEAR:
                        rangeClearBlock();
                        break;
                }
        }

        rangeModeChange();
    }

    int rangemodechangecount;
    bool is_click;

    /// <summary>
    /// 範囲モードを切り替える関数
    /// </summary>
    private void rangeModeChange()
    {
        if (rangemodechangecount > rangemode_change_time)
        {
            is_range_mode = false;
            is_click = false;
            rangemodechangecount = 0;
        }

        if (is_click)
        {
            rangemodechangecount++;
            if (Input.GetMouseButtonDown(0))
            {
                is_range_mode = true;
                is_click = false;
                push_mousepos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

                rangeSetup();
                return;
            }
        }
        if (is_range_mode)
        {
            if (Input.GetMouseButtonUp(0))
            {
                rangemodechangecount = 0;
                is_range_mode = false;
                rangeClear();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            is_click = true;
        }
    }

    GameObject[] rangelines;
    private void rangeSetup()
    {
        GameObject line = new GameObject();
        line.AddComponent<LineRenderer>();
        rangelines = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            rangelines[i] = (GameObject)Instantiate(line, gameObject.transform);
        }
        Destroy(line);
    }

    private void rangeClear()
    {
        for (int i = 0; i < 4; i++)
        {
            Destroy(rangelines[i]);
        }
    }

    Vector3 push_mousepos;
    Vector3 mousepos;

    /// <summary>
    /// 範囲指定の範囲を決める関数
    /// </summary>
    private void rangeDecide()
    {
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos.z = 0;

        var mouse_distance = mousepos - push_mousepos;

        Vector3 rectpos_rightup = new Vector3(push_mousepos.x + mouse_distance.x, push_mousepos.y, 0);
        Vector3 rectpos_leftdown = new Vector3(push_mousepos.x, push_mousepos.y + mouse_distance.y, 0);


        for (int i = 0; i < 4; i++)
        {
            rangelines[i].GetComponent<LineRenderer>().material.color = Color.yellow;
            rangelines[i].GetComponent<LineRenderer>().SetVertexCount(2);
            rangelines[i].GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
        }

        var offset_z = new Vector3(0, 0, -3);

        rangelines[0].GetComponent<LineRenderer>().SetPosition(0, push_mousepos + offset_z);
        rangelines[0].GetComponent<LineRenderer>().SetPosition(1, rectpos_rightup + offset_z);

        rangelines[1].GetComponent<LineRenderer>().SetPosition(0, push_mousepos + offset_z);
        rangelines[1].GetComponent<LineRenderer>().SetPosition(1, rectpos_leftdown + offset_z);

        rangelines[2].GetComponent<LineRenderer>().SetPosition(0, rectpos_rightup + offset_z);
        rangelines[2].GetComponent<LineRenderer>().SetPosition(1, mousepos + offset_z);

        rangelines[3].GetComponent<LineRenderer>().SetPosition(0, rectpos_leftdown + offset_z);
        rangelines[3].GetComponent<LineRenderer>().SetPosition(1, mousepos + offset_z);
    }

    /// <summary>
    /// レイヤーが変わるたびにspriteたちを変える処理
    /// </summary>
    public void changeLayerLoadSprites()
    {
        string layername = uicontroller.currentLayerToString();
        if (current_layername == layername) return;
        current_layername = layername;
        sprites = Resources.LoadAll<Sprite>("Textures/" + EditSelectScene.instance.select_scene + "/" + layername);

        if (layername == "Enemy")
            enemyeditcontroller.enemyLayerChangeSpriteDraw();
        else
            enemyeditcontroller.enemyLayerChangeSpriteClear();
    }

    public void modeWrite()
    {
        editmode = EditMode.WRITE;
    }
    public void modeRemove()
    {
        editmode = EditMode.REMOVE;
    }
    public void modeClear()
    {
        editmode = EditMode.CLEAR;
    }


    /// <summary>
    /// ブロックを置く処理
    /// </summary>
    void editMap()
    {
        var click_obj = getClickObj();
        if (click_obj != null)
        {
            string layername = uicontroller.currentLayerToString();

            // 画像を変える
            var renderer = click_obj.GetComponent<SpriteRenderer>();
            renderer.sprite =
                System.Array.Find<Sprite>(
                    sprites, (sprite) => sprite.name.Equals(
                        layername + "_" + current_select_block_num.ToString()));

            if (layername == "Event")
                click_obj.transform.localScale = new Vector2(6, 6);
            else
                click_obj.transform.localScale = new Vector2(blockcontroller.chip_size, blockcontroller.chip_size);

            // ステータスを変える
            click_obj.GetComponent<BlockStatus>().number = current_select_block_num;
        }
    }

    /// <summary>
    /// 範囲指定バージョンのブロックを置く処理
    /// </summary>
    void rangeEditMap()
    {
        var pushmouse_cell = posToCell(push_mousepos);
        var mouse_cell = mousePosToCell();
        for (int y = (int)pushmouse_cell.y; y < mouse_cell.y; y++)
        {
            if (y < 0 || y > blockcontroller.chip_num_y - 1) continue;

            for (int x = (int)pushmouse_cell.x; x < mouse_cell.x; x++)
            {
                if (x < 0 || x > blockcontroller.chip_num_x - 1) continue;

                var block = blockcontroller.blocks[uicontroller.currentLayerToInt()][y][x];
                string layername = uicontroller.currentLayerToString();
                var renderer = block.GetComponent<SpriteRenderer>();

                renderer.sprite = System.Array.Find<Sprite>(
                                   sprites, (sprite) => sprite.name.Equals(
                                   layername + "_" + current_select_block_num.ToString()));

                if (layername == "Event")
                    block.transform.localScale = new Vector2(6, 6);
                else
                    block.transform.localScale = new Vector2(blockcontroller.chip_size, blockcontroller.chip_size); ;

                block.GetComponent<BlockStatus>().number = current_select_block_num;
            }
        }

    }

    /// <summary>
    /// ブロックのレイヤーを全部消す関数
    /// </summary>
    public void clearBlock()
    {
        var click_obj = getClickObj();
        if (click_obj != null)
        {
            for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
            {
                var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousepos += new Vector3(blockcontroller.chip_start_pos.x, blockcontroller.chip_start_pos.y, 0);
                var mousecell_x = Mathf.FloorToInt(mousepos.x);
                var mousecell_y = Mathf.FloorToInt(mousepos.y) * -1 - 1;

                blockcontroller.blocks[i][mousecell_y][mousecell_x].GetComponent<BlockStatus>().clear();
            }
        }
    }

    /// <summary>
    /// 範囲指定バージョンのレイヤー全消し関数
    /// </summary>
    void rangeClearBlock()
    {
        var pushmouse_cell = posToCell(push_mousepos);
        var mouse_cell = mousePosToCell();

        for (int i = 0; i < (int)UIController.SelectLayer.LAYER_MAX; i++)
        {
            for (int y = (int)pushmouse_cell.y; y < mouse_cell.y; y++)
            {
                if (y < 0 || y > blockcontroller.chip_num_y - 1) continue;

                for (int x = (int)pushmouse_cell.x; x < mouse_cell.x; x++)
                {
                    if (x < 0 || x > blockcontroller.chip_num_x - 1) continue;
                    blockcontroller.blocks[i][y][x].GetComponent<BlockStatus>().clear();

                }
            }
        }
    }

    /// <summary>
    /// 選択中のレイヤーのブロックのみ消す
    /// </summary>
    public void removeBlock()
    {
        var click_obj = getClickObj();
        if (click_obj != null)
        {
            var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos += new Vector3(blockcontroller.chip_start_pos.x, blockcontroller.chip_start_pos.y, 0);
            var mousecell_x = Mathf.FloorToInt(mousepos.x);
            var mousecell_y = Mathf.FloorToInt(mousepos.y) * -1 - 1;

            blockcontroller.blocks[uicontroller.currentLayerToInt()][mousecell_y][mousecell_x].GetComponent<BlockStatus>().clear();
        }
    }

    /// <summary>
    /// 範囲指定バージョンの選択中レイヤーだけ消す関数
    /// </summary>
    void rangeRemoveBlock()
    {
        var pushmouse_cell = posToCell(push_mousepos);
        var mouse_cell = mousePosToCell();


        for (int y = (int)pushmouse_cell.y; y < mouse_cell.y; y++)
        {
            if (y < 0 || y > blockcontroller.chip_num_y - 1) continue;

            for (int x = (int)pushmouse_cell.x; x < mouse_cell.x; x++)
            {
                if (x < 0 || x > blockcontroller.chip_num_x - 1) continue;

                blockcontroller.blocks[uicontroller.currentLayerToInt()][y][x].GetComponent<BlockStatus>().clear();

            }
        }

    }

    /// <summary>
    /// マウスの位置をint型のセルに変換する関数(例外処理はできなかった)
    /// </summary>
    /// <returns>マウスの位置からのセル</returns>
    public Vector2 mousePosToCell()
    {
        var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos += new Vector3(blockcontroller.chip_start_pos.x, blockcontroller.chip_start_pos.y, 0);
        var mousecell_x = Mathf.FloorToInt(mousepos.x);
        var mousecell_y = Mathf.FloorToInt(mousepos.y) * -1 - 1;

        return new Vector2(mousecell_x, mousecell_y);
    }

    Vector2 posToCell(Vector3 pos_)
    {
        pos_ += new Vector3(blockcontroller.chip_start_pos.x, blockcontroller.chip_start_pos.y, 0);
        var mousecell_x = Mathf.FloorToInt(pos_.x);
        var mousecell_y = Mathf.FloorToInt(pos_.y) * -1 - 1;

        return new Vector2(mousecell_x, mousecell_y);
    }

    /// <summary>
    /// マウスの位置からブロックを得る関数
    /// </summary>
    /// <returns>ブロック一個分のGameObject</returns>
    GameObject getHitObj()
    {
        // マウスの位置をスクリーン座標からワールド座標に変換
        var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos += new Vector3(blockcontroller.chip_start_pos.x, blockcontroller.chip_start_pos.y, 0);
        // マウスの位置をint型にキャストしてセルを出す
        var mousecell_x = Mathf.FloorToInt(mousepos.x);
        var mousecell_y = Mathf.FloorToInt(mousepos.y) * -1 - 1;
        if (mousecell_x < 0 || mousecell_y < 0) return null;
        if (mousecell_x > blockcontroller.chip_num_x - 1 || mousecell_y > blockcontroller.chip_num_y - 1) return null;

        return blockcontroller.blocks[uicontroller.currentLayerToInt()][mousecell_y][mousecell_x];
    }

    /// <summary>
    /// マウスがUIに当たっているかどうか
    /// </summary>
    public GameObject getMouseHitUI()
    {
        var pointer = new PointerEventData(EventSystem.current);

        Vector2 mousepos = Input.mousePosition;
        pointer.position = mousepos;

        var raycast_result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycast_result);

        if (raycast_result.Count > 0)
        {
            return raycast_result[0].gameObject;
        }
        return null;
    }

    public GameObject getClickObj()
    {
        if (Input.GetMouseButton(0))
        {
            // マウスがUIに触れていなかったら実行
            if (getMouseHitUI() == null)
                return getHitObj();
        }
        return null;
    }
}
