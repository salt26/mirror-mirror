using UnityEngine;
using System.Collections;

public class TileSelect : MonoBehaviour
{
    public GameObject obj;
    void OnMouseDown()
    {
        if (Editor.validSelected && Editor.selected == myType())
        {
            obj.GetComponent<SpriteRenderer>().color = Color.white;
            Editor.validSelected = false;
        }
        else {
            foreach (GameObject tile in MonoBehaviour.FindObjectOfType<Editor>().tiles)
            {
                tile.GetComponent<SpriteRenderer>().color = Color.white;
            }
            obj.GetComponent<SpriteRenderer>().color = Color.yellow;
            Editor.selected = myType();
            Editor.validSelected = true;
        }
    }

    TileType myType()
    {
        switch (obj.name)
        {
            case "fullcorner":
                return TileType.FullCorner;
            case "halfcorner":
                return TileType.HalfCorner;
            case "fulledge":
                return TileType.FullEdge;
            case "halfedge":
                return TileType.HalfEdge;
            case "start":
                return TileType.Start;
            case "end":
                return TileType.End;
            default:
                return TileType.Empty;
        }
    }
}
