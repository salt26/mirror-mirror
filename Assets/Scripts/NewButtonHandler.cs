using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NewButtonHandler : MonoBehaviour
{
    public void onClick()
    {
        Editor.levelData = null;
        SceneManager.LoadScene("map_editor");
    }
}
