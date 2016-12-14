using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// レイヤーを選ぶボタンを管理するクラス
/// </summary>
public class UIController : MonoBehaviour
{
    public enum SelectLayer
    {
        FLOOR,
        WALL,
        DOOR,
        OBJECT,
        EVENT,
        ENEMY,
        LAYER_MAX,
    }

    public SelectLayer selectlayer = SelectLayer.FLOOR;

    // ボタン押されたら各自呼び出されるinspector最強
    public void selectChangeFloor()
    {
        selectlayer = SelectLayer.FLOOR;
    }
    public void selectChangeDoor()
    {
        selectlayer = SelectLayer.DOOR;
    }
    public void selectChangeWall()
    {
        selectlayer = SelectLayer.WALL;
    }
    public void selectChangeObject()
    {
        selectlayer = SelectLayer.OBJECT;
    }
    public void selectChangeEvent()
    {
        selectlayer = SelectLayer.EVENT;
    }

    [SerializeField]
    EnemyEditController enemyeditcontroller;

    public void selectChangeEnemy()
    {
        if (enemyeditcontroller.is_loadnow == false)
            selectlayer = SelectLayer.ENEMY;
    }


    public int[] prevlayer;
    public SelectLayer currentlayer;

    [SerializeField]
    public Canvas floor_canvas;
    [SerializeField]
    public Canvas wall_canvas;
    [SerializeField]
    public Canvas object_canvas;
    [SerializeField]
    public Canvas event_canvas;
    [SerializeField]
    public Canvas door_canvas;
    [SerializeField]
    public Canvas enemy_canvas;

    // canvasstatus
    // x -390 y -10
    // width 175 height 430

    [SerializeField]
    private CreateSelectableBlocks create_selectable_blocks;

    [SerializeField]
    private MapEditController mapedit_controller;


    void Start()
    {
        prevlayer = new int[(int)SelectLayer.LAYER_MAX];
        prevCanvas();
    }

    void Update()
    {
        if (currentlayer == selectlayer) return;
        // 描画順番変更
        prevCanvas();

        // 選択を消す
        create_selectable_blocks.selectClear();

        // レイヤーが変わったらspriteを読み込みなおす
        mapedit_controller.changeLayerLoadSprites();

        // 今のレイヤー変更
        currentlayer = selectlayer;
    }

    /// <summary>
    /// 描画順番を変更したときに前に表示するcanvasを決める関数
    /// </summary>
    private void prevCanvas()
    {
        for (int i = 0; i < (int)SelectLayer.LAYER_MAX; i++)
        {
            prevlayer[i] = 0;
            if ((int)selectlayer == i)
            {
                prevlayer[i] = 1;
            }
        }

        floor_canvas.sortingOrder = prevlayer[(int)SelectLayer.FLOOR];
        wall_canvas.sortingOrder = prevlayer[(int)SelectLayer.WALL];
        door_canvas.sortingOrder = prevlayer[(int)SelectLayer.DOOR];
        object_canvas.sortingOrder = prevlayer[(int)SelectLayer.OBJECT];
        event_canvas.sortingOrder = prevlayer[(int)SelectLayer.EVENT];
        enemy_canvas.sortingOrder = prevlayer[(int)SelectLayer.ENEMY];
    }

    /// <summary>
    /// 今のレイヤーをintで返す
    /// </summary>
    /// <returns></returns>
    public int currentLayerToInt()
    {
        switch (selectlayer)
        {
            case SelectLayer.FLOOR:
                return (int)SelectLayer.FLOOR;
            case SelectLayer.DOOR:
                return (int)SelectLayer.DOOR;
            case SelectLayer.WALL:
                return (int)SelectLayer.WALL;
            case SelectLayer.OBJECT:
                return (int)SelectLayer.OBJECT;
            case SelectLayer.EVENT:
                return (int)SelectLayer.EVENT;
            case SelectLayer.ENEMY:
                return (int)SelectLayer.ENEMY;
        }
        return -1;
    }

    /// <summary>
    /// 今のレイヤーをstringで返す
    /// </summary>
    /// <returns></returns>
    public string currentLayerToString()
    {
        switch (selectlayer)
        {
            case SelectLayer.FLOOR:
                return "Floor";
            case SelectLayer.DOOR:
                return "Door";
            case SelectLayer.WALL:
                return "Wall";
            case SelectLayer.OBJECT:
                return "Object";
            case SelectLayer.EVENT:
                return "Event";
            case SelectLayer.ENEMY:
                return "Enemy";
        }
        return "";
    }

    /// <summary>
    /// intの値をレイヤーの名前に変換する関数
    /// </summary>
    /// <param name="layernum_"></param>
    /// <returns></returns>
    public string layerToString(int layernum_)
    {
        switch (layernum_)
        {
            case (int)SelectLayer.FLOOR:
                return "Floor";
            case (int)SelectLayer.DOOR:
                return "Door";
            case (int)SelectLayer.WALL:
                return "Wall";
            case (int)SelectLayer.OBJECT:
                return "Object";
            case (int)SelectLayer.EVENT:
                return "Event";
            case (int)SelectLayer.ENEMY:
                return "Enemy";
        }
        return null;
    }
}
