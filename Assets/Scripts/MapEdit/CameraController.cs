using UnityEngine;
using System.Collections;

/// <summary>
/// カメラの操作できるようにするクラス
/// </summary>
public class CameraController : MonoBehaviour
{
    private Camera cameracontroller;

    public float zoom;
    public float zoom_speed;

    void Start()
    {
        zoom = 10;
        zoom_speed = 0.2f;
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
            Vector3 mousepos = Input.mousePosition;
            Vector3 mousevec = (push_mousepos - mousepos) * mousedrag_sens;
            Vector3 camera_pos = mousevec + now_camerapos;
            camera_pos.z = -10;
            cameracontroller.transform.position = camera_pos;
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
            zoom -= zoom_speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            zoom += zoom_speed;
        }

        if (zoom < 1)
            zoom = 1;
        cameracontroller.orthographicSize = zoom;
    }
}
