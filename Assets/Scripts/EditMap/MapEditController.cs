using UnityEngine;
using System.Collections;

/// <summary>
/// マップをエディットするクラス
/// </summary>
public class MapEditController : MonoBehaviour
{
    [SerializeField]
    UIController uicontroller;

    [SerializeField]
    BlockController blockcontroller;

    public int current_select_block_num;
    private int prev_select_block_num;
    Sprite[] sprites;

    void Start()
    {

    }

    void Update()
    {
        changeLayerSprites();
        editMap();
    }

    void changeLayerSprites()
    {
        if (prev_select_block_num == current_select_block_num) return;
        prev_select_block_num = current_select_block_num;

        string layername = uicontroller.currentLayerToString();
        sprites = Resources.LoadAll<Sprite>("Textures/" + layername);
    }

    void editMap()
    {
        var click_obj = blockcontroller.getClickObj();
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
}
