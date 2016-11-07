using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// ボタンUIの色を変えたりするクラス
/// </summary>
public class ButtonColorChange : MonoBehaviour
{

    [SerializeField]
    Button floorbutton;
    [SerializeField]
    Button wallbutton;
    [SerializeField]
    Button objectbutton;
    [SerializeField]
    Button eventbutton;

    [SerializeField]
    UIController uicontroller;

    UIController.SelectLayer currentlayer;

    [SerializeField]
    Button writebutton;
    [SerializeField]
    Button removebutton;
    [SerializeField]
    Button clearbutton;

    [SerializeField]
    MapEditController editcontroller;

    MapEditController.EditMode currenteditmode;

    void Start()
    {
        // 初期値を入れる
        currentlayer = uicontroller.selectlayer;
        layerButtonColorChange();

        currenteditmode = editcontroller.editmode;
        editmodeButtonColorChange();
    }

    void Update()
    {
        // 変更されるたびに色を変える
        if (currentlayer != uicontroller.selectlayer)
        {
            currentlayer = uicontroller.selectlayer;
            layerButtonColorChange();
        }

        if (currenteditmode != editcontroller.editmode)
        {
            currenteditmode = editcontroller.editmode;
            editmodeButtonColorChange();
        }
    }

    void layerButtonColorChange()
    {
        var floorbuttoncolor = floorbutton.colors;
        floorbuttoncolor.normalColor = Color.white;
        floorbuttoncolor.highlightedColor = Color.white;

        var wallbuttoncolor = wallbutton.colors;
        wallbuttoncolor.normalColor = Color.white;
        wallbuttoncolor.highlightedColor = Color.white;

        var objectbuttoncolor = objectbutton.colors;
        objectbuttoncolor.normalColor = Color.white;
        objectbuttoncolor.highlightedColor = Color.white;

        var eventbuttoncolor = eventbutton.colors;
        eventbuttoncolor.normalColor = Color.white;
        eventbuttoncolor.highlightedColor = Color.white;


        switch (uicontroller.selectlayer)
        {
            case UIController.SelectLayer.FLOOR:
                floorbuttoncolor.normalColor = Color.cyan;
                floorbuttoncolor.highlightedColor = Color.cyan;
                break;
            case UIController.SelectLayer.WALL:
                wallbuttoncolor.normalColor = Color.cyan;
                wallbuttoncolor.highlightedColor = Color.cyan;
                break;
            case UIController.SelectLayer.OBJECT:
                objectbuttoncolor.normalColor = Color.cyan;
                objectbuttoncolor.highlightedColor = Color.cyan;
                break;
            case UIController.SelectLayer.EVENT:
                eventbuttoncolor.normalColor = Color.cyan;
                eventbuttoncolor.highlightedColor = Color.cyan;
                break;
            default:
                break;
        }
        floorbutton.colors = floorbuttoncolor;
        wallbutton.colors = wallbuttoncolor;
        objectbutton.colors = objectbuttoncolor;
        eventbutton.colors = eventbuttoncolor;
    }

    void editmodeButtonColorChange()
    {
        var writebuttoncolor = writebutton.colors;
        writebuttoncolor.normalColor = Color.white;
        writebuttoncolor.highlightedColor = Color.white;

        var removebuttoncolor = removebutton.colors;
        removebuttoncolor.normalColor = Color.white;
        removebuttoncolor.highlightedColor = Color.white;

        var clearbuttoncolor = clearbutton.colors;
        clearbuttoncolor.normalColor = Color.white;
        clearbuttoncolor.highlightedColor = Color.white;

        switch (currenteditmode)
        {
            case MapEditController.EditMode.WRITE:
                writebuttoncolor.normalColor = Color.cyan;
                writebuttoncolor.highlightedColor = Color.cyan;
                break;
            case MapEditController.EditMode.REMOVE:
                removebuttoncolor.normalColor = Color.cyan;
                removebuttoncolor.highlightedColor = Color.cyan;
                break;
            case MapEditController.EditMode.CLEAR:
                clearbuttoncolor.normalColor = Color.cyan;
                clearbuttoncolor.highlightedColor = Color.cyan;
                break;
        }
        writebutton.colors = writebuttoncolor;
        removebutton.colors = removebuttoncolor;
        clearbutton.colors = clearbuttoncolor;
    }
}
