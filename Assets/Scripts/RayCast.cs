using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayCast : MonoBehaviour
{
    public GameObject ClearUI; // RayCast에서 들고 있게 했지만 옮길 수 있음
    public LineRenderer ray;
    public Transform laserPrefab;
    public Transform hilightPrefab;
    public bool activeRay = true;
    Map map;
    List<LaserElement> laserList = new List<LaserElement>();
    List<LaserElement> hilightList = new List<LaserElement>();
    float laserLength;
    float hilightTiming = 0;
    float hilightWidth = 8.0f;
    float hilightInterval = 3.76f;
    public static bool isClear;

    // Use this for initialization
    void Start()
    {
        isClear = false;
        laserLength = 0;
    }

    // Update is called once per frame
    void Update()
    {

        hilightList.ForEach(i => Destroy(i.t.gameObject));
        hilightList.Clear();
        if (activeRay)
        {
            float laserPos = 0;
            int hilightCount = 0;
            LaserElement newHilight;
            hilightTiming = (hilightTiming + Time.deltaTime * hilightInterval) % hilightInterval;
            for (int i = 0; i < laserList.Count; i++)
            {
                laserPos += laserList[i].length;
                if (laserPos >= hilightCount * hilightInterval + hilightTiming)
                {
                    newHilight = new LaserElement(hilightPrefab, laserList[i].p, laserList[i].dir, 1.0f);
                    hilightList.Add(newHilight);
                    if (laserList[i].length < 1) newHilight.length = laserList[i].length;
                    MeshRenderer render = newHilight.t.GetComponent<MeshRenderer>();
                    render.material.mainTextureScale = new Vector2(1.0f, hilightWidth * newHilight.length);
                    render.material.mainTextureOffset = new Vector2(0.0f, -hilightWidth * (hilightCount * hilightInterval + hilightTiming + laserList[i].length - laserPos) + 0.5f);
                    render.sortingOrder = laserList.Count;
                    hilightCount++;
                }
            }
        }
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
                // ClearUI.transform.position = new Vector3(0, 0);
                break;
            }
            if (map.tileset.TryGetValue(nextPos, out next))
            {
                if (next.Reflect(dir) == Direction.Empty)
                {
                    break;
                }
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
        laserList.ForEach(i => Destroy(i.t.gameObject));
        laserList.Clear();

        Pos p, nextPos;
        p = map.start.Key;
        Direction dir = map.start.Value.dir;

        while (true)
        {
            Hexagon next;
            nextPos = Hexagon.NextTile(p, dir);
            Debug.DrawLine(Transformer.PosToWorld(p), Transformer.PosToWorld(nextPos), Color.red);
            LaserElement newLaser = new LaserElement(laserPrefab, p, dir, 1.0f);
            laserList.Add(newLaser);
            newLaser.t.parent = ray.transform;
            if (nextPos.Equals(map.end.Key) && dir == map.end.Value.dir)
            {
                // Clear
                Debug.Log("Level Clear");
                isClear = true;
                PlayerPrefs.SetInt(GameLoader.levelData, 1);
                PlayerPrefs.Save();
                FindObjectOfType<UIButtonHandler>().onMenuOpen();
                break;
            }
            if (map.tileset.TryGetValue(nextPos, out next))
            {
                if (next.Reflect(dir) == Direction.Empty)
                {
                    newLaser.length = 0.5f;
                    break;
                }
                dir = next.Reflect(dir);
                p = nextPos;
                if (visited.Contains(new KeyValuePair<Pos, Direction>(p, dir))) break; // Loop
                visited.Add(new KeyValuePair<Pos, Direction>(p, dir));
            }
            else
            {
                newLaser.length = 0.5f;
                break;
            }
        }
        laserLength = 0;
        laserList.ForEach(i => laserLength += i.length);
        for (int i = 0; i < laserList.Count; i++)
        {
            MeshRenderer render = laserList[i].t.GetComponent<MeshRenderer>();
            render.material.mainTextureScale = new Vector2(1.0f, 1/Mathf.Max(16.0f, laserLength));
            render.material.mainTextureOffset = new Vector2(0.0f, i/Mathf.Max(16.0f, laserLength));
            render.sortingOrder = i;
        }
        activeRay = true;
        hilightTiming = 0;
    }

    public void RemoveRay()
    {
        ray.SetVertexCount(1);
        ray.SetPosition(0, Vector3.zero);
        laserList.ForEach(i => Destroy(i.t.gameObject));
        laserList.Clear();
        activeRay = false;
        ray.enabled = false;
        laserLength = 0;
    }
}

class LaserElement
{
    public LaserElement(Transform prefab, Pos p, Direction dir, float length)
    {
        _length = length;
        this.dir = dir;
        this.p = p;
        this.t = (Transform)MonoBehaviour.Instantiate(
                    prefab,
                    (Transformer.PosToWorld(p) * (2 - length) + Transformer.PosToWorld(Hexagon.NextTile(p, dir)) * length) / 2 + (Vector3.back * 0.1f),
                    Quaternion.AngleAxis(Hexagon.DirectionToDegree(dir), Vector3.back));
    }
    public Transform t;
    public Pos p; // 시작 위치
    public Direction dir;
    float _length;
    public float length
    {
        get { return _length; }
        set
        {
            t.position = (Transformer.PosToWorld(p) * (2 - value) + Transformer.PosToWorld(Hexagon.NextTile(p, dir)) * value) / 2 + (Vector3.back * 0.1f);
            t.localScale = t.localScale = new Vector3(t.localScale.x, t.localScale.y * value / _length);
            _length = value;
        }
    }
}


