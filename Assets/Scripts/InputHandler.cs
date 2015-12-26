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
        Hexagon tile;
        if (Input.GetMouseButtonUp(0))
        {
            if (status == MouseStatus.Clicked)
            {
                // Release
                if(FindObjectOfType<GameLoader>().map.tileset.TryGetValue(WorldToPos(mousePos), out released))
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
            if(FindObjectOfType<GameLoader>().map.tileset.TryGetValue(WorldToPos(mousePos), out clicked))
            {
                // 선택
            }
        }
    }

    Pos WorldToPos(Vector3 input)
    {
        // Calculate precise position
        return new Pos((int)input.x, (int)input.y);
    }
}

public enum MouseStatus { Neutral, Clicked }
