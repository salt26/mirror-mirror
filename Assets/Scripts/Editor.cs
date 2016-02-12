using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Editor : MonoBehaviour
{
    private MouseStatus status = MouseStatus.Neutral;
    public static string levelData;
    public static TileType selected;
    public static bool validSelected = false;
    public GameObject[] tiles;
    public Map map;
    private Vector3 start;
    public Direction dir;
    public ArrayList selectedTiles = new ArrayList();
    public GameObject saveButton;
    public GameObject playButton;
    public GameObject backButton;
    public GameObject mapName;
    public GameObject maxFlip;
    public GameObject removeTileButton;
    public GameObject undoButton;
    public static bool play = false;
    public static bool removeTile = false;
    public Stack<KeyValuePair<ArrayList, Direction>> gameStack;
    public LineRenderer ray;

    // Use this for initialization
    void Start()
    {
        int i = 0;
        foreach (GameObject tile in tiles)
        {
            tile.transform.position = new Vector3(-Camera.main.orthographicSize * Screen.width / Screen.height + 1f, Camera.main.orthographicSize - 1f - 1.2f * i, -1f);
            i++;
        }
        if (levelData == null)
        {
            map = new Map();
        }
        else
        {
            map = new Map(levelData);
            map.tileset.Add(map.start.Key, map.start.Value);
            map.tileset.Add(map.end.Key, map.end.Value);
        }
        saveButton.transform.localPosition = new Vector3(Screen.width * 0.5f - 160f, -30f);
        playButton.transform.localPosition = new Vector3(Screen.width * 0.5f - 160f, -60f);
        backButton.transform.localPosition = new Vector3(Screen.width * 0.5f - 160f, -90f);
        mapName.transform.localPosition = new Vector3(Screen.width * 0.5f - 160f, 30f);
        maxFlip.transform.localPosition = new Vector3(Screen.width * 0.5f - 160f, 0f);
        undoButton.transform.localPosition = new Vector3(Screen.width * 0.5f - 160f, 90f);
        removeTileButton.transform.localPosition = new Vector3(-Screen.width * 0.5f + 65f, -Screen.height * 0.5f + 50f);
        gameStack = new Stack<KeyValuePair<ArrayList, Direction>>();

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
        if (play)
        {
            LineRenderer ray = MonoBehaviour.FindObjectOfType<Editor>().ray;
            ray.enabled = true;
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
        else
        {
            LineRenderer ray = MonoBehaviour.FindObjectOfType<Editor>().ray;
            ray.enabled = false;
        }
        Vector3 mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            if (removeTile)
            {
                Hexagon tile;
                if (map.tileset.TryGetValue(Transformer.WorldToPos(mousePos), out tile))
                {
                    DestroyObject(tile.obj);
                    map.tileset.Remove(Transformer.WorldToPos(mousePos));
                }
                removeTile = false;
            }
            if (status == MouseStatus.Neutral)
            {
                start = Transformer.PosToWorld(Transformer.WorldToPos(Input.mousePosition));
            }
            status = MouseStatus.Clicked;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (status == MouseStatus.Clicked)
            {
                if (selectedTiles.Count > 1)
                {
                    foreach (Hexagon tile in selectedTiles)
                    {
                        tile.Flip(dir);
                    }
                    gameStack.Push(new KeyValuePair<ArrayList, Direction>(selectedTiles.Clone() as ArrayList, dir));
                }
                selectedTiles.Clear();
                foreach (Hexagon tile in map.tileset.Values)
                {
                    tile.obj.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
            if (Editor.validSelected)
            {
                if (status == MouseStatus.Clicked && mousePos.x > 120f)
                {
                    if (!map.tileset.ContainsKey(Transformer.WorldToPos(mousePos)))
                    {
                        Hexagon newTile;
                        if (selected == TileType.FullEdge || selected == TileType.HalfEdge)
                        {
                            newTile = new Hexagon(selected, Direction.NNE, Transformer.WorldToPos(mousePos), MonoBehaviour.FindObjectOfType<TileHandler>());
                        }
                        else
                        {
                            newTile = new Hexagon(selected, Direction.North, Transformer.WorldToPos(mousePos), MonoBehaviour.FindObjectOfType<TileHandler>());
                        }

                        if (selected == TileType.Start)
                        {
                            try
                            {
                                DestroyObject(map.start.Value.obj);
                                map.tileset.Remove(map.start.Key);
                            }
                            catch (System.Exception)
                            {
                            }
                            map.start = new KeyValuePair<Pos, Hexagon>(Transformer.WorldToPos(mousePos), newTile);
                        }
                        else if (selected == TileType.End)
                        {
                            try
                            {
                                DestroyObject(map.end.Value.obj);
                                map.tileset.Remove(map.end.Key);
                            }
                            catch (System.Exception)
                            {
                            }
                            map.end = new KeyValuePair<Pos, Hexagon>(Transformer.WorldToPos(mousePos), newTile);
                        }
                        map.tileset.Add(Transformer.WorldToPos(mousePos), newTile);
                        foreach (GameObject tileoption in MonoBehaviour.FindObjectOfType<Editor>().tiles)
                        {
                            tileoption.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        Editor.validSelected = false;
                    }
                    else
                    {
                        Hexagon tile;
                        map.tileset.TryGetValue(Transformer.WorldToPos(mousePos), out tile);
                        if (selected != tile.tile)
                        {
                            MonoBehaviour.Destroy(tile.obj);
                            map.tileset.Remove(Transformer.WorldToPos(mousePos));
                            Hexagon newTile;
                            if (selected == TileType.FullEdge || selected == TileType.HalfEdge)
                            {
                                newTile = new Hexagon(selected, Direction.NNE, Transformer.WorldToPos(mousePos), MonoBehaviour.FindObjectOfType<TileHandler>());
                            }
                            else
                            {
                                newTile = new Hexagon(selected, Direction.North, Transformer.WorldToPos(mousePos), MonoBehaviour.FindObjectOfType<TileHandler>());
                            }
                            map.tileset.Add(Transformer.WorldToPos(mousePos), newTile);
                        }
                        foreach (GameObject tileoption in tiles)
                        {
                            tileoption.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        Editor.validSelected = false;
                    }
                }
            } 
            status = MouseStatus.Neutral;
        }

        if (status == MouseStatus.Clicked)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Debug.DrawLine(start, mouseWorldPos, Color.red);
            Vector3 end;

            float dNorth, dNEE, dEES;
            dNorth = (mouseWorldPos.x - start.x) * (mouseWorldPos.x - start.x);
            dNEE = ((mouseWorldPos.x - start.x) - Mathf.Sqrt(3) * (mouseWorldPos.y - start.y)) * ((mouseWorldPos.x - start.x) - Mathf.Sqrt(3) * (mouseWorldPos.y - start.y)) / 4;
            dEES = ((mouseWorldPos.x - start.x) + Mathf.Sqrt(3) * (mouseWorldPos.y - start.y)) * ((mouseWorldPos.x - start.x) + Mathf.Sqrt(3) * (mouseWorldPos.y - start.y)) / 4;

            if (dNorth < dNEE && dNorth < dEES)
            {
                end = new Vector3(start.x, mouseWorldPos.y);
                if (mouseWorldPos.y > start.y) dir = Direction.North;
                else dir = Direction.South;
            }
            else if (dNEE < dEES)
            {
                float p, q;
                p = start.x - Mathf.Sqrt(3) * start.y;
                q = -Mathf.Sqrt(3) * mouseWorldPos.x - mouseWorldPos.y;
                end = new Vector3(p - Mathf.Sqrt(3) * q, -Mathf.Sqrt(3) * p - q) / 4f;

                if (mouseWorldPos.x > start.x) dir = Direction.NEE;
                else dir = Direction.SWW;
            }
            else
            {
                float p, q;
                p = start.x + Mathf.Sqrt(3) * start.y;
                q = Mathf.Sqrt(3) * mouseWorldPos.x - mouseWorldPos.y;
                end = new Vector3(p + Mathf.Sqrt(3) * q, Mathf.Sqrt(3) * p - q) / 4f;

                if (mouseWorldPos.x > start.x) dir = Direction.EES;
                else dir = Direction.WWN;
            }
            Debug.DrawLine(start, end, Color.blue);

            Hexagon tile;
            Pos pos = Transformer.WorldToPos(Camera.main.WorldToScreenPoint(start));
            foreach (Hexagon t in map.tileset.Values)
            {
                t.obj.GetComponent<SpriteRenderer>().color = Color.white;
            }
            selectedTiles.Clear();
            while (!pos.Equals(Transformer.WorldToPos(Camera.main.WorldToScreenPoint(end))) && map.tileset.TryGetValue(pos, out tile))
            {
                selectedTiles.Add(tile);
                tile.obj.GetComponent<SpriteRenderer>().color = Color.yellow;
                pos = Hexagon.NextTile(pos, dir);
            }
            if (map.tileset.TryGetValue(pos, out tile))
            {
                selectedTiles.Add(tile);
                tile.obj.GetComponent<SpriteRenderer>().color = Color.yellow;
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            Hexagon tile;
            if (map.tileset.TryGetValue(Transformer.WorldToPos(Input.mousePosition), out tile))
            {
                tile.Rotate();
            }
            foreach (GameObject tileoption in tiles)
            {
                tileoption.GetComponent<SpriteRenderer>().color = Color.white;
            }
            Editor.validSelected = false;
        }

        Vector3 camPos = Camera.main.transform.position;
        if (Input.GetKey("up"))
        {
            Camera.main.transform.position = camPos + new Vector3(0f, 0.2f);
        }
        else if (Input.GetKey("down"))
        {
            Camera.main.transform.position = camPos - new Vector3(0f, 0.2f);
        }
        else if (Input.GetKey("left"))
        {
            Camera.main.transform.position = camPos - new Vector3(0.2f, 0f);
        }
        else if (Input.GetKey("right"))
        {
            Camera.main.transform.position = camPos + new Vector3(0.2f, 0f);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > 2)
        {
            Camera.main.orthographicSize -= 0.5f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize += 0.5f;
        }
    }

    public void onUndoClick()
    {
        if (gameStack.Count > 0)
        {
            KeyValuePair<ArrayList, Direction> pop = gameStack.Pop();
            ArrayList tiles = pop.Key;
            Direction dir = pop.Value;
            foreach (Hexagon tile in tiles)
            {
                tile.Flip(dir);
            }
        }
    }
}
