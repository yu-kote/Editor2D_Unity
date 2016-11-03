using UnityEngine;
using System.Collections;

/// <summary>
/// マップをエディットするクラス
/// </summary>
public class MapEditController : MonoBehaviour
{
    [SerializeField]
    UIController uicontroller;

    public int current_select_block_num;

    void Start()
    {

    }

    int cnt = 0;
    void Update()
    {
        cnt++;
        if (cnt % 30 != 0) return;
        //Debug.Log(current_select_block_num);
    }
}
