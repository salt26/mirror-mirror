using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RemoveTileButtonHandler : MonoBehaviour
{
    public void onClick()
    {
        Editor.removeTile = true;
    }
}
