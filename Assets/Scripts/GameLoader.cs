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
        // rayCast.ray.SetVertexCount(1);
        // rayCast.MakeRayLine();
        RayCast.isClear = false;
        rayCast.MakeLaserSprite();

        double minLength = double.PositiveInfinity;
        Pos center = new Pos(0, 0);
        Pos farthest = new Pos(0, 0);
        ArrayList tileList = new ArrayList();
        foreach(Pos p in map.tileset.Keys)
        {
            tileList.Add(p);
        }
        tileList.Add(map.start.Key);
        tileList.Add(map.end.Key);
        foreach(Pos p in tileList)
        {
            double maxLength = 0f;
            Pos localFarthest = new Pos(0, 0);
            foreach(Pos other_p in tileList)
            {
                if (p.Equals(other_p)) continue;
                if (Vector3.Distance(Transformer.PosToWorld(p), Transformer.PosToWorld(other_p)) > maxLength)
                {
                    maxLength = Vector3.Distance(Transformer.PosToWorld(p), Transformer.PosToWorld(other_p));
                    localFarthest = other_p;
                }
            }
            if (maxLength < minLength)
            {
                minLength = maxLength;
                center = p;
                farthest = localFarthest;
            }
        }
        float maxWidth = 0f;
        foreach(Pos p in tileList)
        {
            if (Mathf.Abs(Transformer.PosToWorld(center).x - Transformer.PosToWorld(p).x) > maxWidth)
            {
                maxWidth = Mathf.Abs(Transformer.PosToWorld(center).x - Transformer.PosToWorld(p).x);
            }
        }
        Vector3 initCamPos = Transformer.PosToWorld(center);
        initCamPos.z = -5f;
        initCamPos.y = initCamPos.y + 1f;
        Camera.main.transform.position = initCamPos;
        Camera.main.orthographicSize = maxWidth * 2f + 1f;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
