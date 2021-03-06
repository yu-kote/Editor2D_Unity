﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// セレクトできるブロックを作るクラス
/// </summary>
public class CreateSelectableBlocks : MonoBehaviour
{

    [SerializeField]
    UIController uicontroller;

    public Vector2 startposition;

    public int floor_num;
    public int wall_num;
    public int door_num;
    public int object_num;
    public int event_num;
    public int enemy_num;

    [SerializeField]
    GameObject scroll;
    [SerializeField]
    GameObject scroll_blocklist;
    float offsetscroll_blocklist_y;

    void Start()
    {
        var parentframe_position = uicontroller.floor_canvas.transform.position;

        startposition = new Vector2(50, -130) + (Vector2)parentframe_position;
        offsetscroll_blocklist_y = 0.0f;

        createSelectableBlocks(out selectable_floor_blocks, ref uicontroller.floor_canvas, "Floor", floor_num, startposition);
        createSelectableBlocks(out selectable_wall_blocks, ref uicontroller.wall_canvas, "Wall", wall_num, startposition);
        createSelectableBlocks(out selectable_door_blocks, ref uicontroller.door_canvas, "Door", door_num, startposition);
        createSelectableBlocks(out selectable_object_blocks, ref uicontroller.object_canvas, "Object", object_num, startposition);
        createSelectableBlocks(out selectable_event_blocks, ref uicontroller.event_canvas, "Event", event_num, startposition);
        createEnemySelectableBlocks(out selectable_enemy_blocks, ref uicontroller.enemy_canvas, "Enemy", enemy_num, startposition + new Vector2(100, -20));
    }

    void Update()
    {
        blockNumberUpdate();
    }

    [SerializeField]
    MapEditController editcontroller;

    Button[] selectable_floor_blocks;
    Button[] selectable_wall_blocks;
    Button[] selectable_door_blocks;
    Button[] selectable_object_blocks;
    Button[] selectable_event_blocks;
    Button[] selectable_enemy_blocks;

    public int return_x_count;
    public int size_x;
    public int size_y;



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
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/" + EditSelectScene.instance.select_scene + "/" + sprite_name_);

        scroll_blocklist.transform.position += new Vector3(0, offsetscroll_blocklist_y, 0);


        int x = 0;
        int y = 0;
        for (int i = 0; i < maxnum_; i++)
        {
            // PrefabのButtonBaseをもとに作る
            var button = Resources.Load<Button>("Prefabs/ButtonBase");

            // spriteを入れる
            {
                Image buttonimage = button.image;
                buttonimage.sprite = null;

                Sprite temp = System.Array.Find<Sprite>(
                                        sprites, (sprite) => sprite.name.Equals(
                                            sprite_name_ + "_" + i.ToString()));

                buttonimage.sprite = temp;
                buttonimage.rectTransform.sizeDelta = new Vector2(size_x, size_y);
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

            selectable_block_[i] = (Button)Instantiate(button);

            if (maxnum_ > 75)
            {
                selectable_block_[i].transform.SetParent(scroll.transform, false);
            }
            else
                selectable_block_[i].transform.SetParent(canvas_.transform, false);

        }
    }

    /// <summary>
    /// エネミーのボタンを例外処理にする
    /// </summary>
    void createEnemySelectableBlocks(out Button[] selectable_block_, ref Canvas canvas_, string sprite_name_, int maxnum_, Vector2 start_position_)
    {
        // 配列の要素数の関係で+1する必要がある
        maxnum_ += 1;
        selectable_block_ = new Button[maxnum_];
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/" + EditSelectScene.instance.select_scene + "/" + sprite_name_);

        int x = 0;
        int y = 0;
        for (int i = 0; i < maxnum_; i++)
        {
            // PrefabのButtonBaseをもとに作る
            var button = Resources.Load<Button>("Prefabs/ButtonBase");

            // spriteを入れる
            {
                Image buttonimage = button.image;
                buttonimage.sprite = null;

                Sprite temp = System.Array.Find<Sprite>(
                                        sprites, (sprite) => sprite.name.Equals(
                                            sprite_name_ + "_" + i.ToString()));


                buttonimage.sprite = temp;
                buttonimage.rectTransform.sizeDelta = new Vector2(size_x, size_y);
            }

            // buttonを並べる
            {
                RectTransform buttonrect = button.transform as RectTransform;
                Vector2 size = buttonrect.sizeDelta + new Vector2(20.0f, 1.5f);
                Vector2 offsetposition = new Vector2(size.x * x, size.y * -y);
                buttonrect.anchorMin = new Vector2(0, 1);
                buttonrect.anchorMax = new Vector2(0, 1);

                x++;
                if (x != 0 && x % 2 == 0)
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

            selectable_block_[i] = (Button)Instantiate(button);
            selectable_block_[i].transform.SetParent(canvas_.transform, false);

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
            case UIController.SelectLayer.DOOR:
                return selectable_door_blocks;
            case UIController.SelectLayer.OBJECT:
                return selectable_object_blocks;
            case UIController.SelectLayer.EVENT:
                return selectable_event_blocks;
            case UIController.SelectLayer.ENEMY:
                return selectable_enemy_blocks;

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

    /// <summary>
    /// 前選んでいたボタンをfalseにして、新しく選んだほうをtrueにする
    /// </summary>
    /// <param name="current_select_block_num_"></param>
    /// <param name="new_select_block_num_"></param>
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
            case UIController.SelectLayer.DOOR:
                selectable_door_blocks[current_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = false;
                selectable_door_blocks[new_select_block_num_].GetComponent<ButtonBlockStatus>()
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
            case UIController.SelectLayer.ENEMY:
                selectable_enemy_blocks[current_select_block_num_].GetComponent<ButtonBlockStatus>()
                    .is_select = false;
                selectable_enemy_blocks[new_select_block_num_].GetComponent<ButtonBlockStatus>()
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


    /// <summary>
    /// レイヤーを変えたときに選んでいたブロックをfalseにする
    /// </summary>
    public void selectClear()
    {
        switch (uicontroller.currentlayer)
        {
            case UIController.SelectLayer.FLOOR:
                for (int i = 0; i < selectable_floor_blocks.Length; i++)
                {
                    selectable_floor_blocks[i].GetComponent<ButtonBlockStatus>().is_select = false;
                }
                break;
            case UIController.SelectLayer.WALL:
                for (int i = 0; i < selectable_wall_blocks.Length; i++)
                {
                    selectable_wall_blocks[i].GetComponent<ButtonBlockStatus>().is_select = false;
                }
                break;
            case UIController.SelectLayer.DOOR:
                for (int i = 0; i < selectable_door_blocks.Length; i++)
                {
                    selectable_door_blocks[i].GetComponent<ButtonBlockStatus>().is_select = false;
                }
                break;
            case UIController.SelectLayer.OBJECT:
                for (int i = 0; i < selectable_object_blocks.Length; i++)
                {
                    selectable_object_blocks[i].GetComponent<ButtonBlockStatus>().is_select = false;
                }
                break;
            case UIController.SelectLayer.EVENT:
                for (int i = 0; i < selectable_event_blocks.Length; i++)
                {
                    selectable_event_blocks[i].GetComponent<ButtonBlockStatus>().is_select = false;
                }
                break;
            case UIController.SelectLayer.ENEMY:
                for (int i = 0; i < selectable_enemy_blocks.Length; i++)
                {
                    selectable_enemy_blocks[i].GetComponent<ButtonBlockStatus>().is_select = false;
                }
                break;
        }
        editcontroller.current_select_block_num = 0;

    }
}

