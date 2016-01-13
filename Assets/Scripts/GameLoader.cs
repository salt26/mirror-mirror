﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    public Map map;
    private Transform mapHolder;
    public static string levelData;

    // Use this for initialization
    void Start()
    {
        map = new Map(levelData);
        mapHolder = new GameObject("Map").transform;
        foreach (Hexagon tile in map.tileset.Values)
        {
            tile.obj.transform.SetParent(mapHolder);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
