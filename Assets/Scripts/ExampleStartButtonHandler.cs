using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExampleStartButtonHandler : MonoBehaviour
{
    public void onClick()
    {
        GameLoader.levelData = "example";
        SceneManager.LoadScene("gameplay");
    }
}
