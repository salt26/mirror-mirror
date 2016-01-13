using UnityEngine;
using System.Collections;

public class Editor : MonoBehaviour
{
    private MouseStatus status = MouseStatus.Neutral;
    public static string levelData;
    public static TileType selected;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            if (status == MouseStatus.Neutral)
            {
                
            }
            status = MouseStatus.Clicked;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (status == MouseStatus.Clicked)
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
            }
            status = MouseStatus.Neutral;
        }
    }
}
