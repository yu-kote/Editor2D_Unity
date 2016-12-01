using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


/// <summary>
/// 配置するブロックを選ぶUIクラス(使ってない)
/// </summary>
public class BlockSelectUI : MonoBehaviour
{

    void Start()
    {
        is_hitUI = false;
    }

    // UIにマウスが当たっているかどうか
    public bool is_hitUI;

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
    
}
