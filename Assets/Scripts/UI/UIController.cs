using UnityEngine;
using System.Collections;
using System;


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
    private SelectLayer currentlayer;

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


    void Start()
    {
        prevlayer = new int[(int)SelectLayer.LAYER_MAX];
        prevCanvas();
    }

    void Update()
    {
        if (currentlayer == selectlayer) return;
        currentlayer = selectlayer;
        prevCanvas();
    }

    /// <summary>
    /// 描画順番を変更したときに前に表示するcanvasを決める関数
    /// </summary>
    private void prevCanvas()
    {
        for (int i = 0; i < (int)SelectLayer.LAYER_MAX; i++)
        {
            prevlayer[i] = 0;
            if ((int)currentlayer == i)
            {
                prevlayer[i] = 1;
            }
        }

        floor_canvas.sortingOrder = prevlayer[0];
        wall_canvas.sortingOrder = prevlayer[1];
        object_canvas.sortingOrder = prevlayer[2];
        event_canvas.sortingOrder = prevlayer[3];
    }
}
