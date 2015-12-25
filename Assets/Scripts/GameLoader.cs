using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour {
    public Map map;
    public GameObject[] tiles;
    private Transform mapHolder;
    private const float scale = 2f;
    private const float xOffset = -1f;
    private const float yOffset = -2f;

	// Use this for initialization
	void Start () {
        map = new Map();
        mapHolder = new GameObject("Map").transform;
        foreach (KeyValuePair<Pos, Hexagon> tile in map.tileset)
        {
            GameObject tileInstance = Instantiate(tiles[(int)tile.Value.tile],
                PosToWorld(tile.Key), Quaternion.AngleAxis(Hexagon.DirectionToDegree(tile.Value.dir), Vector3.back)) as GameObject;
            tileInstance.transform.SetParent(mapHolder);
        }
	}

    Vector3 PosToWorld(Pos p)
    {
        return new Vector3(xOffset + p.x * 2.33f / 3f, yOffset + ((p.x % 2 == 0) ? p.y : p.y + 0.5f) * Mathf.Sqrt(3f) / 2f, 0f) * scale;
    }
	
	// Update is called once per frame
	void Update () {
	}
}
