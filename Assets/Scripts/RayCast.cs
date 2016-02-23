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
    float hilightInterval = 5.63f;
    float hilightSpeed = 4f;
    public static bool isClear;
    public static bool playingClearAnimation;
    AudioSource clearSound;
    public AudioClip clearSoundClip;

    // Use this for initialization
    void Start()
    {
        isClear = false;
        playingClearAnimation = false;
        RemoveRay();
        clearSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        hilightList.ForEach(i => Destroy(i.t.gameObject));
        hilightList.Clear();
        if (activeRay && !playingClearAnimation)
        {
            float laserPos = 0;
            int hilightCount = 0;
            LaserElement newHilight;
            hilightTiming = (hilightTiming + Time.deltaTime * hilightSpeed) % hilightInterval;
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
                /*
                PlayerPrefs.SetInt(GameLoader.levelData, 1);
                PlayerPrefs.Save();
                FindObjectOfType<UIButtonHandler>().onMenuOpen();
                */
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
        if (isClear) StartCoroutine(StageClear());
    }

    IEnumerator StageClear()
    {
        LaserElement clearHilight = new LaserElement(hilightPrefab, laserList[0].p, laserList[0].dir, 1.0f); ;
        PlayerPrefs.SetInt(GameLoader.levelData, 1);
        PlayerPrefs.Save();
        playingClearAnimation = true;
        for (int i = 0; i < laserList.Count; i++)
        {
            laserList[i].t.gameObject.SetActive(false);
        }
        for (float clearLaserPos = 0; clearLaserPos < laserLength; clearLaserPos += Time.deltaTime * 6.0f)
        {
            if (!UIButtonHandler.clearAnimation) break;
            float drawedLaserPos = 0;
            for (int i = 0; i < laserList.Count; i++)
            {
                if (clearLaserPos - drawedLaserPos <= 0)
                    break;
                else
                {
                    laserList[i].t.gameObject.SetActive(true);
                    if (clearLaserPos - drawedLaserPos <= 1.0f)
                    {
                        laserList[i].length = clearLaserPos - drawedLaserPos;
                        clearHilight.SetPosition(laserList[i].p, laserList[i].dir, laserList[i].length);
                        MeshRenderer render = clearHilight.t.GetComponent<MeshRenderer>();
                        render.material.mainTextureScale = new Vector2(1.0f, hilightWidth * clearHilight.length);
                        render.material.mainTextureOffset = new Vector2(0.0f, -hilightWidth * clearHilight.length + 0.8f);
                        render.sortingOrder = laserList.Count;
                    }
                    else if (laserList[i].length < 1.0f)
                        laserList[i].length = 1.0f;
                }
                drawedLaserPos += laserList[i].length;
            }
            yield return null;
        }
        for (int i = 0; i < laserList.Count; i++)
        {
            laserList[i].t.gameObject.SetActive(true);
            laserList[i].length = 1.0f;
        }
        Destroy(clearHilight.t.gameObject);
        if (UIButtonHandler.clearAnimation == true)
        {
            Transform t = (Transform)MonoBehaviour.Instantiate(
                    hilightPrefab,
                    Transformer.PosToWorld(Hexagon.NextTile(laserList[laserList.Count-1].p, laserList[laserList.Count - 1].dir)) + (Vector3.back * 0.3f),
                    Quaternion.AngleAxis(Hexagon.DirectionToDegree(laserList[laserList.Count - 1].dir) + 90, Vector3.back));
            t.localScale = new Vector3(0.4f, 1.30f, 1.0f);
            Material m = t.GetComponent<MeshRenderer>().material;
            /*
            for (float i = 0.1f; i < 0.7f; i += Time.deltaTime)
            {
                m.mainTextureScale = new Vector2(1, 0.1f / i / i);
                m.mainTextureOffset = new Vector2(0, 0.5f - 0.1f / i / i / 2);
                yield return null;
            }
            for(float i = 0.7f; i > 0.1; i -= Time.deltaTime)
            {
                m.mainTextureScale = new Vector2(1, 0.1f / i / i);
                m.mainTextureOffset = new Vector2(0, 0.5f - 0.1f / i / i / 2);
                yield return null;
            }
            */
            m.mainTextureScale = new Vector2(1f, 3f);
            float i = 0.0f;
            for (; i < 0.3f; i += Time.deltaTime)
            {
                m.mainTextureOffset = new Vector2(0, -7 * i + 3f);
                yield return null;
            }
            clearSound.PlayOneShot(clearSoundClip, 0.3f);
            for (; i < 1f; i += Time.deltaTime)
            {
                m.mainTextureOffset = new Vector2(0, -7 * i + 3f);
                yield return null;
            }
            Destroy(t.gameObject);
        }
        playingClearAnimation = false;
        FindObjectOfType<UIButtonHandler>().onMenuOpen();
        FindObjectOfType<UIButtonHandler>().menuUI.SetActive(false);
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
        _length = 1.0f;
        this.dir = dir;
        this.p = p;
        this.t = (Transform)MonoBehaviour.Instantiate(
                    prefab,
                    (Transformer.PosToWorld(p) * (2 - length) + Transformer.PosToWorld(Hexagon.NextTile(p, dir)) * length) / 2 + (Vector3.back * 0.1f),
                    Quaternion.AngleAxis(Hexagon.DirectionToDegree(dir), Vector3.back));
        this.length = length;
    }

    public void SetPosition(Pos p, Direction dir, float length)
    {
        this.dir = dir;
        this.p = p;
        this.length = length;
        this.t.rotation = Quaternion.AngleAxis(Hexagon.DirectionToDegree(dir), Vector3.back);
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
            t.localScale = new Vector3(t.localScale.x, t.localScale.y * value / _length);
            _length = value;
        }
    }
}


