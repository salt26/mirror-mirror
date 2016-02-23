﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour {
    public GameObject detailUI;
    public static Vector3 detailUIPos;

    void Start()
    {
        detailUIPos = detailUI.transform.position;
    }

    public void OnMouseDown()
    {
        if (!LevelSelectLoader.isUIVisible)
        {
            detailUI.transform.position = this.transform.position;
            FindObjectOfType<LevelSelectLoader>().targetMap = this.gameObject;
            detailUI.transform.Find("MapSelect/mapRect").GetComponent<Image>().sprite = this.transform.GetComponent<Image>().sprite;
            detailUI.transform.Find("MapSelect/mapRect/thumbnail").GetComponent<Image>().sprite = this.transform.Find("thumbnail").GetComponent<Image>().sprite;
            detailUI.transform.Find("MapSelect/mapRect/ClearStar").GetComponent<Image>().sprite = this.transform.Find("ClearStar").GetComponent<Image>().sprite;
            detailUI.transform.Find("MapSelect/mapRect/MapNumber").GetComponent<Text>().text = this.transform.Find("MapNumber").GetComponent<Text>().text;
            StartCoroutine("PopUp");
            LevelSelectLoader.isUIVisible = true;
        }
    }

    IEnumerator PopUp()
    {
        for (float f = 0f; f < 1.1f; f += 0.1f)
        {
            detailUI.transform.localScale = new Vector3(f, f) * 1.2f;
            detailUI.transform.position = (detailUIPos * f + this.transform.position * (1f - f));
            yield return null;
        }
    }
}
