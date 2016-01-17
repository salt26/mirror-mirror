using UnityEngine;
using System.Collections;

public class PlayButtonHandler : MonoBehaviour
{
    public UnityEngine.UI.Button self;
    public void onClick()
    {
        if (Editor.play)
        {
            Editor.play = false;
            self.GetComponentInChildren<UnityEngine.UI.Text>().text = "플레이";
        }
        else
        {
            Editor.play = true;
            self.GetComponentInChildren<UnityEngine.UI.Text>().text = "플레이 중지";
        }
    }
}
