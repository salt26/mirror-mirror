using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartSceneButtonHandler : MonoBehaviour
{
    public void onGameStartClick()
    {
        SceneManager.LoadScene("map_select_final");
    }

    public void onExitClick()
    {
        Application.Quit();
    }

    public void onHowtoClick()
    {
    }
}