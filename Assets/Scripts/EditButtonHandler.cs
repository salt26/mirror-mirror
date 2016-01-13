using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EditButtonHandler : MonoBehaviour
{
    public void onClick()
    {
        UnityEngine.UI.Dropdown dropdown = MonoBehaviour.FindObjectOfType<UnityEngine.UI.Dropdown>();
        string level = dropdown.options[dropdown.value].text;
        Editor.levelData = level;
        SceneManager.LoadScene("map_editor");
    }
}