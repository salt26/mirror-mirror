using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    public void onClick()
    {
        SceneManager.LoadScene("map_select");
    }
}
