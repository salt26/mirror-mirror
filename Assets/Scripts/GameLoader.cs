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
        ClearUI.transform.localPosition = new Vector3(Screen.width * 3f, Screen.height * 3f); //Clear시 UI가 옮겨짐
        // rayCast.ray.SetVertexCount(1);
        // rayCast.MakeRayLine();
        rayCast.MakeLaserSprite();

        double minLength = double.PositiveInfinity;
        Pos center = new Pos(0, 0);
        foreach(Pos p in map.tileset.Keys)
        {
            double maxLength = 0f;
            foreach(Pos other_p in map.tileset.Keys)
            {
                if (p.Equals(other_p)) continue;
                if (Vector3.Distance(Transformer.PosToWorld(p), Transformer.PosToWorld(other_p)) > maxLength)
                {
                    maxLength = Vector3.Distance(Transformer.PosToWorld(p), Transformer.PosToWorld(other_p));
                }
            }
            if (maxLength < minLength)
            {
                minLength = maxLength;
                center = p;
            }
        }
        Vector3 initCamPos = Transformer.PosToWorld(center);
        initCamPos.z = -5f;
        Camera.main.transform.position = initCamPos;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
