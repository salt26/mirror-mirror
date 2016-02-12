using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveButtonHandler : MonoBehaviour
{
    public Text mapNameInputField;
    public Text maxFlipInputField;

    public void onClick()
    {
        Map map = MonoBehaviour.FindObjectOfType<Editor>().map;
        string filename = mapNameInputField.text.ToString();
        int maxFlip;
        bool start = false, end = false;
        if (filename == "")
        {
            Debug.LogError("Filename is Empty");
        }
        else if (!Int32.TryParse(maxFlipInputField.text.ToString(), out maxFlip))
        {
            Debug.LogError("Invalid number of Max Flip");
        }
        else {
            XmlDocument xmldoc = new XmlDocument();
            string filepath = Application.dataPath.ToString() + "/Resources/level/" + filename + ".xml";
            Debug.Log(filepath);
            XmlElement wrapper = xmldoc.CreateElement("map");
            XmlElement maxFlipElem = xmldoc.CreateElement("flip");
            maxFlipElem.InnerText = maxFlip.ToString();
            wrapper.AppendChild(maxFlipElem);
            foreach (Pos p in map.tileset.Keys)
            {
                Hexagon tile;
                if (map.tileset.TryGetValue(p, out tile))
                {
                    XmlElement e;
                    if (tile.tile == TileType.Start)
                    {
                        if (start)
                        {
                            Debug.LogError("More than one start");
                            return;
                        }
                        start = true;
                        e = xmldoc.CreateElement("start");
                    }
                    else if (tile.tile == TileType.End)
                    {
                        if (end)
                        {
                            Debug.LogError("More than one end");
                            return;
                        }
                        end = true;
                        e = xmldoc.CreateElement("end");
                    }
                    else {
                        e = xmldoc.CreateElement("tile");

                        XmlElement type = xmldoc.CreateElement("type");
                        type.InnerText = tile.tile.ToString();
                        e.AppendChild(type);
                    }
                    XmlElement pos_x = xmldoc.CreateElement("pos_x");
                    pos_x.InnerText = p.x.ToString();
                    XmlElement pos_y = xmldoc.CreateElement("pos_y");
                    pos_y.InnerText = p.y.ToString();
                    XmlElement dir = xmldoc.CreateElement("dir");
                    dir.InnerText = tile.dir.ToString();
                    e.AppendChild(pos_x);
                    e.AppendChild(pos_y);
                    e.AppendChild(dir);
                    wrapper.AppendChild(e);
                }
            }
            if (start && end)
            {
                xmldoc.AppendChild(wrapper);
                xmldoc.Save(filepath);
            }
            else
            {
                Debug.LogError("One start and end tile each is required");
                return;
            }
        }
    }
}
