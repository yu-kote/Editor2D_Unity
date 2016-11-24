using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// セーブを実行するクラス
/// </summary>
public class Save : MonoBehaviour
{

    /// <summary>
    /// 保存ボタンが押されたときの処理
    /// </summary>
    public void clickSaveButton()
    {
        input_savename.gameObject.SetActive(true);
        is_exit = !is_exit;
    }



    [SerializeField]
    InputField input_savename;

    [SerializeField]
    GameObject save_message;

    [SerializeField]
    BlockController blockcontroller;

    int message_count;
    bool is_nowsave;

    bool is_exit;

    bool is_overwrite;
    string overwrite_name;

    void Start()
    {
        input_savename.gameObject.SetActive(false);
        message_count = 0;
        is_overwrite = false;
        is_nowsave = false;
        is_exit = false;
    }

    void Update()
    {
        if (input_savename.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                input_savename.gameObject.SetActive(false);
                save_message.SetActive(true);
                // セーブする関数呼び出し
                blockcontroller.saveFunc(input_savename.text);
                // セーブしましたメッセージ表示
                save_message.GetComponent<Text>().text = "'" + input_savename.text + "'を保存しました。";
                is_overwrite = true;
                overwrite_name = input_savename.text;
                is_nowsave = true;
            }
            if (is_exit == false)
            {
                input_savename.gameObject.SetActive(false);
                is_exit = true;
            }
        }
        else
        {
            is_exit = false;
        }


        if (is_nowsave)
        {
            message_count++;
            if (message_count > 100)
            {
                message_count = 0;
                is_nowsave = false;
                save_message.GetComponent<Text>().text = "失敗。";
                save_message.SetActive(false);
            }
        }

        // 一度セーブすれば ctrl + s でセーブできる
        if (is_overwrite)
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    input_savename.gameObject.SetActive(false);
                    save_message.SetActive(true);
                    // セーブする関数呼び出し
                    blockcontroller.saveFunc(overwrite_name);
                    // セーブしましたメッセージ表示
                    save_message.GetComponent<Text>().text = "'" + overwrite_name + "'を保存しました。";
                }
            }
    }
}
