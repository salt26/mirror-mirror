using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject clearUI;
    public static string[] levelList;

    public void onResumeClick()
    {
        menuUI.SetActive(false);
    }

    public void onNextLevelClick()
    {
        int i = 0;
        while (GameLoader.levelData != levelList[i++]) ;
        GameLoader.levelData = levelList[i];
        SceneManager.LoadScene("gameplay");
    }

    public void onViewMapClick()
    {
        clearUI.SetActive(false);
    }

    public void onMenuOpen()
    {
        if (RayCast.isClear) clearUI.SetActive(true);
        else menuUI.SetActive(true);
    }
}