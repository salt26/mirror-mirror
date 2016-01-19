using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    public Map map;
    private Transform mapHolder;
    public static string levelData;
    public GameObject undoButton;
    public GameObject ClearUI;
    public LineRenderer ray;

    // Use this for initialization
    void Start()
    {
        map = new Map(levelData);
        mapHolder = new GameObject("Map").transform;
        foreach (Hexagon tile in map.tileset.Values)
        {
            tile.obj.transform.SetParent(mapHolder);
        }
        undoButton.transform.localPosition = new Vector3(Screen.width * 0.5f - 160f, 0f);
        ClearUI.transform.localPosition = new Vector3(Screen.width * 3f, Screen.height * 3f); //Clear시 UI가 옮겨짐
        ray.SetVertexCount(1);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
