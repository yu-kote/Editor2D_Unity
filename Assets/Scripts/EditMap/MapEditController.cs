using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// マップをエディットするクラス
/// </summary>
public class MapEditController : MonoBehaviour
{
    [SerializeField]
    UIController uicontroller;

    [SerializeField]
    BlockController blockcontroller;

    // 今選んでいるブロック番号
    public int current_select_block_num;
    // 前選んでいたブロック番号
    private int prev_select_block_num;

    Sprite[] sprites;

    void Start()
    {
        current_select_block_num = 0;
        prev_select_block_num = -1;
    }

    void Update()
    {
        changeLayerLoadSprites();
        editMap();
    }

    public void changeLayerLoadSprites()
    {
        string layername = uicontroller.currentLayerToString();
        sprites = Resources.LoadAll<Sprite>("Textures/" + layername);
    }

    void editMap()
    {
        var click_obj = getClickObj();
        if (click_obj != null)
        {
            string layername = uicontroller.currentLayerToString();

            var renderer = click_obj.GetComponent<SpriteRenderer>();
            renderer.sprite =
                System.Array.Find<Sprite>(
                    sprites, (sprite) => sprite.name.Equals(
                        layername + "_" + current_select_block_num.ToString()));
        }
    }



    /// <summary>
    /// マウスの位置からブロックを得る関数
    /// </summary>
    /// <returns>ブロック一個分のGameObject</returns>
    GameObject getHitObj()
    {
        // マウスの位置をスクリーン座標からワールド座標に変換
        var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
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
            if (getMouseHitUI() == null)
                return getHitObj();
        }
        return null;
    }
}
