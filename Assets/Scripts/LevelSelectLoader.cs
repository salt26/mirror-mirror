using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectLoader : MonoBehaviour
{
    public GameObject detailUI;
    public static bool isUIVisible;
    public GameObject targetMap;
    public GameObject mapSelectHanlder;

    // Use this for initialization
    void Start()
    {
        Initialize();
        loadLevel();
    }

    void loadLevel()
    {
        UIButtonHandler.levelList = new string[16];
        for(int i = 0; i < 15; i++)
        {
            Transform mapSlot = mapSelectHanlder.transform.GetChild(i).GetChild(0);
            if (PlayerPrefs.GetInt((i / 4 + 1).ToString() + "-" + (i % 4 + 1).ToString()) == 1)
            {
                mapSlot.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("img/BlueStar");
            }
            else
            {
                mapSlot.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("img/DottedBlueStar");
            }
            UIButtonHandler.levelList[i] = (i / 4 + 1).ToString() + "-" + (i % 4 + 1).ToString();
        }
        UIButtonHandler.levelList[15] = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        detailUI.transform.localScale = new Vector3(0f, 0f);
        isUIVisible = false;
    }

    public void onCloseClick()
    {
        isUIVisible = false;
        StartCoroutine("PopUpDown");
    }

    IEnumerator PopUpDown()
    {
        for (float f = 1f; f > -0.1f; f -= 0.1f)
        {
            detailUI.transform.localScale = new Vector3(f, f) * 0.76f;
            detailUI.transform.position = (MapSelect.detailUIPos * f + targetMap.transform.position * (1f - f));
            yield return null;
        }
    }

    public void onPlayClick()
    {
        GameLoader.levelData = targetMap.transform.name;
        if (GameLoader.levelData == "1-1")
        {
            SceneManager.LoadScene("tutorial1");
        }
        else if (GameLoader.levelData == "1-2")
        {
            SceneManager.LoadScene("tutorial2");
        }
        else if (GameLoader.levelData == "1-3")
        {
            SceneManager.LoadScene("tutorial3");
        }
        else {
            SceneManager.LoadScene("gameplay");
        }
    }
}