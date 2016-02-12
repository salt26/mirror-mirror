using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    public Map map;
    private Transform mapHolder;
    public static string levelData;
    public GameObject undoButton;
    public GameObject backButton;
    public GameObject ClearUI;
    public RayCast rayCast;

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
        backButton.transform.localPosition = new Vector3(Screen.width * 0.5f - 160f, -30f);
        ClearUI.transform.localPosition = new Vector3(Screen.width * 3f, Screen.height * 3f); //Clear시 UI가 옮겨짐
        // rayCast.ray.SetVertexCount(1);
        // rayCast.MakeRayLine();
        rayCast.MakeLaserSprite();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
