using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonBlockStatus : MonoBehaviour
{
    public int number = 0;
    public bool is_select = false;

    public void selectBlock()
    {
        is_select = true;
    }

    [SerializeField]
    Button button;

    void Start()
    {
        // PrefabにHierarchyのGameObjectを入れられなかったので、
        // フラグで管理することとする
        // だいぶごり押し
        button.onClick.AddListener(selectBlock);
    }

    void Update()
    {

    }
}
