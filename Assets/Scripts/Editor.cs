using UnityEngine;
using System.Collections;

public class Editor : MonoBehaviour
{
    private MouseStatus status = MouseStatus.Neutral;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        if (Input.GetMouseButtonUp(0))
        {
            if (status == MouseStatus.Clicked)  // 릴리즈
            {
                // 
            }
            status = MouseStatus.Neutral;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            status = MouseStatus.Clicked;
        }

        if (status == MouseStatus.Clicked)
        {
        }
    }
}
