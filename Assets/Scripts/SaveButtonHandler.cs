using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class SaveButtonHandler : MonoBehaviour
{
    public void onClick()
    {
        XmlDocument xmldoc = new XmlDocument();
        string filepath = Application.dataPath.ToString() + "/Resources/level/" + MonoBehaviour.FindObjectOfType<UnityEngine.UI.InputField>().text.ToString() + ".xml";
        Debug.Log(filepath);
        Map map = MonoBehaviour.FindObjectOfType<Editor>().map;
        XmlElement wrapper = xmldoc.CreateElement("map");
        foreach (Pos p in map.tileset.Keys)
        {
            Hexagon tile;
            if (map.tileset.TryGetValue(p, out tile))
            {
                XmlElement e = xmldoc.CreateElement("tile");

                XmlElement pos_x = xmldoc.CreateElement("pos_x");
                pos_x.InnerText = p.x.ToString();
                XmlElement pos_y = xmldoc.CreateElement("pos_y");
                pos_y.InnerText = p.y.ToString();
                XmlElement type = xmldoc.CreateElement("type");
                type.InnerText = tile.tile.ToString();
                XmlElement dir = xmldoc.CreateElement("dir");
                dir.InnerText = tile.dir.ToString();
                e.AppendChild(pos_x);
                e.AppendChild(pos_y);
                e.AppendChild(type);
                e.AppendChild(dir);
                wrapper.AppendChild(e);
            }
        }
        xmldoc.AppendChild(wrapper);
        xmldoc.Save(filepath);
    }
}
