using UnityEngine;
using System.Collections;

/// <summary>
/// ブロックの番号を持っているクラス
/// </summary>
public class BlockStatus : MonoBehaviour
{

    public int number;


    /// <summary>
    /// ブロックを空白にする関数
    /// </summary>
    public void clear()
    {
        var block = gameObject;

        number = -1;
        var renderer = block.GetComponent<SpriteRenderer>();
        renderer.sprite = null;
    }

}
