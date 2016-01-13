using UnityEngine;
using System.Collections;

public class TileSelect : MonoBehaviour
{
    public GameObject obj;
    public GameObject[] others;
    void OnMouseDown()
    {
        foreach(GameObject tile in others)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.white;
        }
        obj.GetComponent<SpriteRenderer>().color = Color.yellow;
        switch (obj.name)
        {
            case "empty":
                Editor.selected = TileType.Empty;
                break;
            case "fullcorner":
                Editor.selected = TileType.FullCorner;
                break;
            case "halfcorner":
                Editor.selected = TileType.HalfCorner;
                break;
            case "fulledge":
                Editor.selected = TileType.FullEdge;
                break;
            case "halfedge":
                Editor.selected = TileType.HalfEdge;
                break;
        }
    }
}
