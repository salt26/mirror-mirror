using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonHandler : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject clearUI;
    public GameObject[] clearButtons;
    public static string[] levelList;
    public static bool clearAnimation;

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
        if (RayCast.isClear)
        {
            if (!clearUI.activeSelf)
            {
                clearUI.SetActive(true);
                StartCoroutine(clearUIPopup());
            }
            else
            {
                clearAnimation = false;
            }
        }
        else menuUI.SetActive(true);
        int i = 0;
        while (GameLoader.levelData != levelList[i++]) ;
        if (levelList[i] == "")
        {
            clearUI.transform.GetChild(3).GetComponent<Button>().interactable = false;
        }
    }

    IEnumerator clearUIPopup()
    {
        Vector3 origPos = clearUI.GetComponent<RectTransform>().localPosition;
        foreach (GameObject button in clearButtons)
        {
            button.SetActive(false);
        }
        float f = 0.3f;
        clearUI.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 200f * f - 100f);
        clearUI.GetComponent<RectTransform>().localPosition = origPos - new Vector3(0f, 100f * f - 100f);

        for (f = 0f; f < 1f; f += Time.deltaTime)
        {
            if (!clearAnimation) f = 1f;
            yield return null;
        }

        for (f = 0.3f; f < 1f; f += Time.deltaTime * 1.5f)
        {
            if (!clearAnimation) f = 1f;
            clearUI.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 200f * f - 100f);
            clearUI.GetComponent<RectTransform>().localPosition = origPos - new Vector3(0f, 100f * f - 100f);
            if (f > 0.4f)
            {
                clearButtons[0].SetActive(true);
            }
            if (f > 0.6f)
            {
                clearButtons[1].SetActive(true);
            }
            if (f > 0.8f)
            {
                clearButtons[2].SetActive(true);
            }
            yield return null;
        }
        clearButtons[3].SetActive(true);
        clearAnimation = false;
    }
}