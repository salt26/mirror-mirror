using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    private MouseStatus status = MouseStatus.Neutral;
    private Vector3 start;
    private bool isCleared = false;
    public ArrayList selectedTiles = new ArrayList();
    public Direction dir;
    public Stack<KeyValuePair<ArrayList, Direction>> gameStack;
    public GameObject PlayUI;
    public GameObject ClearUI;

    void Start()
    {
        gameStack = new Stack<KeyValuePair<ArrayList, Direction>>();
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        if (!isCleared)
        {
            if (Input.GetMouseButtonUp(0))
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
                    foreach (Hexagon tile in MonoBehaviour.FindObjectOfType<GameLoader>().map.tileset.Values)
                    {
                        tile.obj.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
                status = MouseStatus.Neutral;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Pos p = Transformer.WorldToPos(mousePos);
                if (MonoBehaviour.FindObjectOfType<GameLoader>().map.tileset.ContainsKey(p))
                {
                    start = Transformer.PosToWorld(p);
                    status = MouseStatus.Clicked;
                }
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
                foreach (Hexagon t in MonoBehaviour.FindObjectOfType<GameLoader>().map.tileset.Values)
                {
                    t.obj.GetComponent<SpriteRenderer>().color = Color.white;
                }
                selectedTiles.Clear();
                while (!pos.Equals(Transformer.WorldToPos(Camera.main.WorldToScreenPoint(end))) && MonoBehaviour.FindObjectOfType<GameLoader>().map.tileset.TryGetValue(pos, out tile))
                {
                    selectedTiles.Add(tile);
                    tile.obj.GetComponent<SpriteRenderer>().color = Color.yellow;
                    pos = Hexagon.NextTile(pos, dir);
                }
                if (MonoBehaviour.FindObjectOfType<GameLoader>().map.tileset.TryGetValue(pos, out tile))
                {
                    selectedTiles.Add(tile);
                    tile.obj.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > 2)
        {
            Camera.main.orthographicSize -= 0.5f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize += 0.5f;
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

    public void clearStage() // inputHandler가 들고 있게 함
    {
        isCleared = true;
        PlayUI.transform.localPosition = new Vector3(Screen.width * 3f, Screen.height * 3f); //Clear시 UI가 옮겨짐
        ClearUI.transform.localPosition = new Vector3(0, 0);
    }
}

public enum MouseStatus { Neutral, Clicked }
