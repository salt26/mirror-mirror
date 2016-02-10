﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayCast : MonoBehaviour
{
    public GameObject ClearUI; // RayCast에서 들고 있게 했지만 옮길 수 있음
    public LineRenderer ray;
    public Transform laserPrefab;
    public bool activeRay = true;
    Map map;
    List<Transform> laserList = new List<Transform>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MakeRayLine()
    {
        map = MonoBehaviour.FindObjectOfType<GameLoader>().map;
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
                // ClearUI.transform.localPosition = new Vector3(0, 0);
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
        activeRay = true;
        ray.enabled = true;
    }

    public void MakeLaserSprite()
    {
        map = MonoBehaviour.FindObjectOfType<GameLoader>().map;
        ArrayList visited = new ArrayList();
        laserList.ForEach(i => Destroy(i.gameObject));
        laserList.Clear();

        Pos p, nextPos;
        p = map.start.Key;
        Direction dir = map.start.Value.dir;

        while (true)
        {
            Hexagon next;
            nextPos = Hexagon.NextTile(p, dir);
            Debug.DrawLine(Transformer.PosToWorld(p), Transformer.PosToWorld(nextPos), Color.red);
            Transform newLaser = (Transform) Instantiate(
                laserPrefab, 
                (Transformer.PosToWorld(p) + Transformer.PosToWorld(nextPos)) / 2 + (Vector3.back * 0.2f), 
                Quaternion.AngleAxis(Hexagon.DirectionToDegree(dir), Vector3.back));
            laserList.Add(newLaser);
            newLaser.parent = ray.transform;
            if (nextPos.Equals(map.end.Key) && dir == map.end.Value.dir)
            {
                // Clear
                Debug.Log("Level Clear");
                // ClearUI.transform.localPosition = new Vector3(0, 0);
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
        
        activeRay = true;
    }

    public void RemoveRay()
    {
        ray.SetVertexCount(1);
        ray.SetPosition(0, Vector3.zero);
        laserList.ForEach(i => Destroy(i.gameObject));
        laserList.Clear();
        activeRay = false;
        ray.enabled = false;
    }
}
