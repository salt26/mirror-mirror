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
        Debug.Log(Application.dataPath);
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/level");
        FileInfo[] levels = dir.GetFiles("*.xml");
        List<string> nameList = new List<string>();
        foreach (FileInfo level in levels)
        {
            nameList.Add(Path.GetFileNameWithoutExtension(level.Name));
        }
        mapList.ClearOptions();
        mapList.AddOptions(nameList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
