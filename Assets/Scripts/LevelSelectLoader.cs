﻿using UnityEngine;
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
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/level/levelList.xml");
        String textAsset = sr.ReadToEnd();
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset);

        string[] names = { "tutorial", "easy", "normal", "hard" };
        UIButtonHandler.levelList = new string[16];
        for(int i = 0; i < 4; i++)
        {
            XmlNodeList tutorials = xmldoc.SelectSingleNode("levels/" + names[i]).SelectNodes("title");
            for (int j = 0; j < 4; j++)
            {
                Transform mapSlot = mapSelectHanlder.transform.GetChild(4 * i + j).GetChild(0);
                if (tutorials[j] != null)
                {
                    Sprite thumbnail = Resources.Load<Sprite>("img/thumbnail/" + tutorials[j].InnerText);
                    if (thumbnail != null)
                    {
                        mapSlot.GetChild(0).GetComponent<Image>().sprite = thumbnail;
                    }
                    else
                    {
                        mapSlot.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("img/closeX");
                    }
                    mapSlot.name = tutorials[j].InnerText;

                    if ( PlayerPrefs.GetInt(tutorials[j].InnerText) == 1)
                    {
                        mapSlot.GetChild(1).GetComponent<Text>().text = "CLEAR";
                    }
                    else
                    {
                        mapSlot.GetChild(1).GetComponent<Text>().text = "";
                    }
                }
                else
                {
                    mapSlot.GetChild(1).GetComponent<Text>().text = "";
                    mapSlot.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("img/lock");
                    mapSlot.name = "";
                }
                UIButtonHandler.levelList[4 * i + j] = mapSlot.name;
            }
        }
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
        SceneManager.LoadScene("gameplay");
    }
}