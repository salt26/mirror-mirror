using UnityEngine;
using System.Collections;

public class Transformer
{
    public const float scale = 2f;
    public const float xOffset = -1f;
    public const float yOffset = -2f;

    public static Vector3 PosToWorld(Pos p)
    {
        return new Vector3(xOffset + p.x * 0.75f, yOffset + ((p.x % 2 == 0) ? p.y : p.y + 0.5f) * Mathf.Sqrt(3f) / 2f, 0f) * scale;
    }

    public static Pos WorldToPos(Vector3 input)
    {
        int x, y;
        float worldX, worldY;

        worldX = Camera.main.ScreenToWorldPoint(input).x - xOffset * scale + 1f;
        worldY = Camera.main.ScreenToWorldPoint(input).y - yOffset * scale + 1f;

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