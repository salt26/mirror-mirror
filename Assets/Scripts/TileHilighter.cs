using UnityEngine;
using System.Collections;

public class TileHilighter : MonoBehaviour {
    public Material tileMat;
    public Material hilightMat;

    public void Hilight()
    {
        GetComponent<MeshRenderer>().material = hilightMat;
    }

    public void Dehilight()
    {
        GetComponent<MeshRenderer>().material = tileMat;
    }

}
