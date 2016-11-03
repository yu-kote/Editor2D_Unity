using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    void Start()
    {
        currentlayer = uicontroller.selectlayer;
        buttonColorChange();
    }

    void Update()
    {
        if (currentlayer != uicontroller.selectlayer)
        {
            currentlayer = uicontroller.selectlayer;
            buttonColorChange();
        }
    }

    void buttonColorChange()
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
                floorbuttoncolor = floorbutton.colors;
                floorbuttoncolor.normalColor = Color.cyan;
                floorbuttoncolor.highlightedColor = Color.cyan;
                break;
            case UIController.SelectLayer.WALL:
                wallbuttoncolor = wallbutton.colors;
                wallbuttoncolor.normalColor = Color.cyan;
                wallbuttoncolor.highlightedColor = Color.cyan;
                break;
            case UIController.SelectLayer.OBJECT:
                objectbuttoncolor = objectbutton.colors;
                objectbuttoncolor.normalColor = Color.cyan;
                objectbuttoncolor.highlightedColor = Color.cyan;
                break;
            case UIController.SelectLayer.EVENT:
                eventbuttoncolor = eventbutton.colors;
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
}
