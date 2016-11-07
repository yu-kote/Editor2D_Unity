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
        OBJECT,
        EVENT,
        LAYER_MAX,
    }

    public SelectLayer selectlayer = SelectLayer.FLOOR;

    // ボタン押されたら各自呼び出されるinspector最強
    public void selectChangeFloor()
    {
        selectlayer = SelectLayer.FLOOR;
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

        floor_canvas.sortingOrder = prevlayer[0];
        wall_canvas.sortingOrder = prevlayer[1];
        object_canvas.sortingOrder = prevlayer[2];
        event_canvas.sortingOrder = prevlayer[3];
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
            case SelectLayer.WALL:
                return (int)SelectLayer.WALL;
            case SelectLayer.OBJECT:
                return (int)SelectLayer.OBJECT;
            case SelectLayer.EVENT:
                return (int)SelectLayer.EVENT;
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
            case SelectLayer.WALL:
                return "Wall";
            case SelectLayer.OBJECT:
                return "Object";
            case SelectLayer.EVENT:
                return "Event";
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
            case (int)SelectLayer.WALL:
                return "Wall";
            case (int)SelectLayer.OBJECT:
                return "Object";
            case (int)SelectLayer.EVENT:
                return "Event";
        }
        return null;
    }
}
