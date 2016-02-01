using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitButtonHandler : MonoBehaviour
{
    public void onClick()
    {
        Application.Quit();
    }
}
