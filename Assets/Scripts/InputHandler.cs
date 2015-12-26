using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    private Hexagon clicked, released;
    private MouseStatus status = MouseStatus.Neutral;

    void Start()
    {

    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        /*
        if (Input.GetMouseButtonUp(0))
        {
            if (status == MouseStatus.Clicked)
            {
                // Release
                if (FindObjectOfType<GameLoader>().map.tileset.TryGetValue(WorldToPos(mousePos), out released))
                {
                    if (clicked != released)
                    {

                    }
                }
            }
            status = MouseStatus.Neutral;
        }
        if (Input.GetMouseButtonDown(0))
        {
            status = MouseStatus.Clicked;
            Pos p = WorldToPos(mousePos);
            Debug.Log(p.x + ", " + p.y);
            if (FindObjectOfType<GameLoader>().map.tileset.TryGetValue(WorldToPos(mousePos), out clicked))
            {
                // 선택
            }
        }
        */
    }

    public static Pos WorldToPos(Vector3 input)
    {
        int x, y;
        float worldX, worldY;

        worldX = (input.x / Screen.width * 10.0f - 5.0f) * Screen.width / Screen.height - GameLoader.xOffset * GameLoader.scale + 1f;
        worldY = (input.y / Screen.height * 10.0f - 5.0f) - GameLoader.yOffset * GameLoader.scale + 1f;

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
                if (worldY - Mathf.Sqrt(3) * y > -Mathf.Sqrt(3) * (worldX - (x * 3f / 2f) - 5f / 2f)) { x++; y++; }
                if (worldY - Mathf.Sqrt(3) * y < Mathf.Sqrt(3) * (worldX - (x * 3f / 2f) - 3f / 2f)) { x++; }
            }
        }
        return new Pos(x, y);
    }
}

public enum MouseStatus { Neutral, Clicked }
