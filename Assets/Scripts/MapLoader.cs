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
        TextAsset[] levels = Resources.LoadAll<TextAsset>("level");
        List<string> nameList = new List<string>();
        foreach (TextAsset level in levels)
        {
            nameList.Add(Path.GetFileNameWithoutExtension(level.name));
        }
        mapList.ClearOptions();
        mapList.AddOptions(nameList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
