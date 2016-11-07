using UnityEngine;
using System.Collections;

/// <summary>
/// クリックの処理順を調べるテストクラス
/// </summary>
public class ClickTest : MonoBehaviour
{

    public void click()
    {
        Debug.Log("onclick");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse_down");
        }

        // この間にUIのonclickが呼ばれる
        // click();

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("mouse_up");
        }
    }
}
