using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour
{
    public UnityEngine.UI.Dropdown mapList;

    // Use this for initialization
    void Start()
    {
        string[] maps = Directory.GetFiles("Assets/Resources/level", "*.xml");
        List<string> nameList = new List<string>();
        foreach (string mapFile in maps)
        {
            nameList.Add(Path.GetFileNameWithoutExtension(mapFile));
        }
        mapList.ClearOptions();
        mapList.AddOptions(nameList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
