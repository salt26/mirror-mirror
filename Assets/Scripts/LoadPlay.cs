using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadPlay : MonoBehaviour {
    public void onClick()
    {
        UnityEngine.UI.Dropdown dropdown = MonoBehaviour.FindObjectOfType<UnityEngine.UI.Dropdown>();
        string level = dropdown.options[dropdown.value].text;
        GameLoader.levelData = level;
        SceneManager.LoadScene("gameplay");
    }
}
