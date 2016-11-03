using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CreateSelectableBlocks : MonoBehaviour
{

    [SerializeField]
    UIController uicontroller;

    void Start()
    {
        var parentframe_position = uicontroller.floor_canvas.transform.position;

        Vector2 startposition = new Vector2(-450, 165) + (Vector2)parentframe_position;

        createSelectableBlocks(out selectable_floor_blocks, ref uicontroller.floor_canvas, "Floor", 23, startposition);
        createSelectableBlocks(out selectable_wall_blocks, ref uicontroller.wall_canvas, "Wall", 1, startposition);
        createSelectableBlocks(out selectable_object_blocks, ref uicontroller.object_canvas, "Object", 1, startposition);
        createSelectableBlocks(out selectable_event_blocks, ref uicontroller.event_canvas, "", 23, startposition);

    }

    // Update is called once per frame
    void Update()
    {
        blockNumberUpdate();
    }

    [SerializeField]
    MapEditController editcontroller;

    Button[] selectable_floor_blocks;
    Button[] selectable_wall_blocks;
    Button[] selectable_object_blocks;
    Button[] selectable_event_blocks;



    /// <summary>
    /// 選べるブロックを生成する関数
    /// </summary>
    /// <param name="selectable_blocks_">選べるブロックの配列</param>
    /// <param name="canvas_">SpriteをAddするCanvas</param>
    /// <param name="sprite_name_">Spriteの名前</param>
    /// <param name="maxnum_">Spriteの数</param>
    /// <param name="start_position_">Spriteの数</param>
    public void createSelectableBlocks(out Button[] selectable_block_, ref Canvas canvas_, string sprite_name_, int maxnum_, Vector2 start_position_)
    {
        // 配列の要素数の関係で+1する必要がある
        maxnum_ += 1;
        selectable_block_ = new Button[maxnum_];
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/" + sprite_name_);

        int return_x_count = 6;
        int x = 0;
        int y = 0;
        for (int i = 0; i < maxnum_; i++)
        {
            // PrefabのButtonBaseをもとに作る
            var button = Resources.Load<Button>("Prefabs/ButtonBase");

            // spriteを入れる
            {
                Image buttonimage = button.image;
                buttonimage.sprite = //.overrideSprite =
                    System.Array.Find<Sprite>(
                            sprites, (sprite) => sprite.name.Equals(
                                sprite_name_ + "_" + i.ToString()));
            }

            // buttonを並べる
            {
                RectTransform buttonrect = button.transform as RectTransform;
                Vector2 size = buttonrect.sizeDelta + new Vector2(1.5f, 1.5f);
                Vector2 offsetposition = new Vector2(size.x * x, size.y * -y);
                buttonrect.anchorMin = new Vector2(0, 1);
                buttonrect.anchorMax = new Vector2(0, 1);

                x++;
                if (x != 0 && x % return_x_count == 0)
                {
                    y++;
                    x = 0;
                }

                var buttontransform = button.transform as RectTransform;
                buttontransform.position = start_position_ + offsetposition;
            }

            // 番号を割り当てる
            {
                button.GetComponent<ButtonBlockStatus>().number = i;
            }

            selectable_block_[i] = (Button)Instantiate(button, canvas_.transform);
        }
    }

    /// <summary>
    /// 今選んでいるレイヤーのButton配列を返す
    /// </summary>
    /// <returns></returns>
    public Button[] getSelectLayerButtons()
    {
        switch (uicontroller.selectlayer)
        {
            case UIController.SelectLayer.FLOOR:
                return selectable_floor_blocks;
            case UIController.SelectLayer.WALL:
                return selectable_wall_blocks;
            case UIController.SelectLayer.OBJECT:
                return selectable_object_blocks;
            case UIController.SelectLayer.EVENT:
                return selectable_event_blocks;
        }
        return null;
    }

    /// <summary>
    /// 新しく選んだ番号を返す
    /// </summary>
    /// <returns>新しい番号(変わってなかったら同じ番号)</returns>
    public int getNewNumber()
    {
        Button[] selectlayerbuttons = getSelectLayerButtons();
        for (int i = 0; i < getSelectLayerButtons().Length; i++)
        {
            var button_status = selectlayerbuttons[i].GetComponent<ButtonBlockStatus>();
            var is_select = button_status.is_select;
            if (!is_select)
                continue;

            var num = button_status.number;
            if (editcontroller.current_select_block_num != num)
                return num;
        }
        return editcontroller.current_select_block_num;
    }

    public void currentSelectButtonBlockChange(int current_select_block_num_, int new_select_block_num_)
    {
        switch (uicontroller.selectlayer)
        {
            case UIController.SelectLayer.FLOOR:
                selectable_floor_blocks[current_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = false;
                selectable_floor_blocks[new_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = true;
                break;
            case UIController.SelectLayer.WALL:
                selectable_wall_blocks[current_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = false;
                selectable_wall_blocks[new_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = true;
                break;
            case UIController.SelectLayer.OBJECT:
                selectable_object_blocks[current_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = false;
                selectable_object_blocks[new_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = true;
                break;
            case UIController.SelectLayer.EVENT:
                selectable_event_blocks[current_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = false;
                selectable_event_blocks[new_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = true;
                break;
        }
    }

    /// <summary>
    /// ButtonBlockの番号更新関数
    /// </summary>
    public void blockNumberUpdate()
    {
        if (!Input.GetMouseButtonUp(0)) return;
        var new_number = getNewNumber();
        var current_number = editcontroller.current_select_block_num;
        if (current_number == new_number) return;

        currentSelectButtonBlockChange(current_number, new_number);
        editcontroller.current_select_block_num = new_number;
    }
}

