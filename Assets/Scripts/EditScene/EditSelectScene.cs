using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EditSelectScene : MonoBehaviour
{
    public static EditSelectScene instance = new EditSelectScene();

    public string select_scene = null;


    public void selectSchool()
    {
        instance.select_scene = "School";
        SceneManager.LoadScene("Editor");
    }

    public void selectHospital()
    {
        instance.select_scene = "Hospital";
        SceneManager.LoadScene("Editor");
    }

    public void selectMansion()
    {
        instance.select_scene = "Videl";
        SceneManager.LoadScene("Editor");
    }
}
