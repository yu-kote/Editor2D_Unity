using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createText()
    {
        var test = new Rect(new Vector3(0, 0, 0), new Vector2(1, 1));
        GUI.TextArea(test, "TestText");
    }
}
