using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// データのロードを実行するクラス
/// </summary>
public class Load : MonoBehaviour
{

    /// <summary>
    /// 保存ボタンが押されたときの処理
    /// </summary>
    public void clickLoadButton()
    {
        //gameObject.SetActive(false);
        input_loadname.gameObject.SetActive(true);
        is_exit = !is_exit;
    }

    [SerializeField]
    InputField input_loadname;

    [SerializeField]
    GameObject load_message;

    [SerializeField]
    BlockController blockcontroller;

    bool is_nowload;
    int message_count;

    bool is_exit;

    void Start()
    {
        input_loadname.gameObject.SetActive(false);
        message_count = 0;
        is_nowload = false;
        is_exit = false;
    }

    void Update()
    {
        if (input_loadname.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                input_loadname.gameObject.SetActive(false);
                load_message.SetActive(true);
                // ロードする関数呼び出し
                blockcontroller.loadFunc(input_loadname.text);
                // ロードしましたメッセージ表示
                load_message.GetComponent<Text>().text = "'" + input_loadname.text + "'を読み込みました。";
                is_nowload = true;
            }
            if (is_exit == false)
            {
                input_loadname.gameObject.SetActive(false);
                is_exit = true;
            }
        }
        else
        {
            is_exit = false;
        }


        if (is_nowload)
        {
            message_count++;
            if (message_count > 100)
            {
                is_nowload = false;
                message_count = 0;
                load_message.GetComponent<Text>().text = "失敗。";
                load_message.SetActive(false);
            }
        }
    }
}
