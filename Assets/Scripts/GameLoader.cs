using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    public Map map;
    private Transform mapHolder;
    public GameObject[] tiles;
    public const float scale = 2f;
    public const float xOffset = -1f;
    public const float yOffset = -2f;

    // Use this for initialization
    void Start()
    {
        map = new Map();
        mapHolder = new GameObject("Map").transform;
        foreach (Hexagon tile in map.tileset.Values)
        {
            tile.obj.transform.SetParent(mapHolder);
        }
    }

    public static Vector3 PosToWorld(Pos p)
    {
        return new Vector3(xOffset + p.x * 0.75f, yOffset + ((p.x % 2 == 0) ? p.y : p.y + 0.5f) * Mathf.Sqrt(3f) / 2f, 0f) * scale;
    }

    // Update is called once per frame
    void Update()
    {
        Hexagon tile;
        foreach (Hexagon t in map.tileset.Values)
        {
            t.obj.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (map.tileset.TryGetValue(InputHandler.WorldToPos(Input.mousePosition), out tile))
        {
            tile.obj.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}
