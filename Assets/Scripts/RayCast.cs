using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayCast : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Map map = MonoBehaviour.FindObjectOfType<GameLoader>().map;
        LineRenderer ray = MonoBehaviour.FindObjectOfType<GameLoader>().ray;
        ArrayList visited = new ArrayList();
        ArrayList rayPoints = new ArrayList();

        Pos p, nextPos;
        p = map.start.Key;
        Direction dir = map.start.Value.dir;
        rayPoints.Add(Transformer.PosToWorld(p));

        while (true)
        {
            Hexagon next;
            nextPos = Hexagon.NextTile(p, dir);
            rayPoints.Add(Transformer.PosToWorld(nextPos));
            Debug.DrawLine(Transformer.PosToWorld(p), Transformer.PosToWorld(nextPos), Color.red);
            if (nextPos.Equals(map.end.Key) && dir == map.end.Value.dir)
            {
                // Clear
                Debug.Log("Level Clear");
                break;
            }
            if (map.tileset.TryGetValue(nextPos, out next))
            {
                dir = next.Reflect(dir);
                p = nextPos;
                if (visited.Contains(new KeyValuePair<Pos, Direction>(p, dir))) break; // Loop
                visited.Add(new KeyValuePair<Pos, Direction>(p, dir));
            }
            else break;
        }

        ray.SetVertexCount(rayPoints.Count);
        ray.SetPositions((Vector3[])(rayPoints.ToArray(typeof(Vector3))));
    }
}
