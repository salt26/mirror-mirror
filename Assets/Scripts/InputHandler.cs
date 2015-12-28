using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    private MouseStatus status = MouseStatus.Neutral;
    public const float scale = 2f;
    public const float xOffset = -1f;
    public const float yOffset = -2f;
    private Vector3 start;
    public ArrayList selectedTiles = new ArrayList();
    public Direction dir;

    void Start()
    {

    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
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
            Pos p = WorldToPos(mousePos);
            if (MonoBehaviour.FindObjectOfType<GameLoader>().map.tileset.ContainsKey(p))
            {
                start = PosToWorld(p);
                status = MouseStatus.Clicked;
            }
        }

        if (status == MouseStatus.Clicked)
        {
            Vector3 mouseWorldPos = MouseToWorld(mousePos);
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
            Pos pos = WorldToPos(Camera.main.WorldToScreenPoint(start));
            foreach (Hexagon t in MonoBehaviour.FindObjectOfType<GameLoader>().map.tileset.Values)
            {
                t.obj.GetComponent<SpriteRenderer>().color = Color.white;
            }
            selectedTiles.Clear();
            while (!pos.Equals(WorldToPos(Camera.main.WorldToScreenPoint(end))) && MonoBehaviour.FindObjectOfType<GameLoader>().map.tileset.TryGetValue(pos, out tile))
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

    public static Vector3 MouseToWorld(Vector3 p)
    {
        float worldX, worldY;
        worldX = (p.x / Screen.width * 10.0f - 5.0f) * Screen.width / Screen.height;
        worldY = (p.y / Screen.height * 10.0f - 5.0f);
        return new Vector3(worldX, worldY);
    }

    public static Vector3 PosToWorld(Pos p)
    {
        return new Vector3(xOffset + p.x * 0.75f, yOffset + ((p.x % 2 == 0) ? p.y : p.y + 0.5f) * Mathf.Sqrt(3f) / 2f, 0f) * scale;
    }

    public static Pos WorldToPos(Vector3 input)
    {
        int x, y;
        float worldX, worldY;

        worldX = (input.x / Screen.width * 10.0f - 5.0f) * Screen.width / Screen.height - xOffset * scale + 1f;
        worldY = (input.y / Screen.height * 10.0f - 5.0f) - yOffset * scale + 1f;

        if (((int)(worldX * 2 - 1) % 3 + 3) % 3 < 2)
        {
            x = Mathf.FloorToInt((worldX * 2 - 1) / 3);
            y = (x % 2 == 0) ? Mathf.FloorToInt(worldY / Mathf.Sqrt(3)) : Mathf.FloorToInt((worldY / Mathf.Sqrt(3)) - 0.5f);
        }
        else
        {
            x = Mathf.FloorToInt((worldX * 2 - 1) / 3);
            y = (x % 2 == 0) ? Mathf.FloorToInt(worldY / Mathf.Sqrt(3)) : Mathf.FloorToInt((worldY / Mathf.Sqrt(3)) - 0.5f);
            if (x % 2 == 0)
            {
                if (worldY - Mathf.Sqrt(3) * y > -Mathf.Sqrt(3) * (worldX - (x * 3f / 2f) - 5f / 2f)) { x++; }
                if (worldY - Mathf.Sqrt(3) * y < Mathf.Sqrt(3) * (worldX - (x * 3f / 2f) - 3f / 2f)) { x++; y--; }
            }
            else
            {
                if (worldY - Mathf.Sqrt(3) / 2f - Mathf.Sqrt(3) * y > -Mathf.Sqrt(3) * (worldX - (x * 3f / 2f) - 5f / 2f)) { x++; y++; }
                if (worldY - Mathf.Sqrt(3) / 2f - Mathf.Sqrt(3) * y < Mathf.Sqrt(3) * (worldX - (x * 3f / 2f) - 3f / 2f)) { x++; }
            }
        }
        return new Pos(x, y);
    }
}

public enum MouseStatus { Neutral, Clicked }
