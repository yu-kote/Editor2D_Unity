using UnityEngine;
using System.Collections;

/// <summary>
/// カメラの操作できるようにするクラス
/// </summary>
public class CameraController : MonoBehaviour
{
    private Camera cameracontroller;

    public float zoom;

    void Start()
    {
        zoom = 10;
        cameracontroller = gameObject.GetComponent<Camera>();

    }

    Vector3 push_mousepos = new Vector3();
    Vector3 now_camerapos = new Vector3(0, 0, 0);
    bool is_mousedrag;
    [SerializeField]
    public float mousedrag_sens;

    Vector3 screenpoint;
    Vector3 offset;
    Vector3 mousepos_;

    void Update()
    {
        cameraMove();
        cameraReset();
        cameraZoom();




    }

    /// <summary>
    /// 右クリックで操作
    /// </summary>
    void cameraMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            push_mousepos = Input.mousePosition;
            is_mousedrag = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            is_mousedrag = false;
            now_camerapos = cameracontroller.transform.position;
        }

        if (is_mousedrag)
        {
            var offsetz = new Vector3(0, 0, 10);
            Vector3 mousepos = Input.mousePosition;
            Vector3 mousevec = (push_mousepos - mousepos) * mousedrag_sens;
            cameracontroller.transform.position = mousevec + now_camerapos - offsetz;
        }
    }


    void cameraReset()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(0, 0, 0 - 10);
        }
    }

    void cameraZoom()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            zoom -= 0.1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            zoom += 0.1f;
        }
        cameracontroller.orthographicSize = zoom;
    }
}
