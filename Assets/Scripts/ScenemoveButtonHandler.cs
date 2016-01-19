using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenemoveButtonHandler : MonoBehaviour
{
    public string sceneName;
    public void onClick()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
