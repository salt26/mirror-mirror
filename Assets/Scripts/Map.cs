using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class Map
{
    public Dictionary<Pos, Hexagon> tileset = new Dictionary<Pos, Hexagon>();
    public KeyValuePair<Pos, Hexagon> start, end;

    public Map() { }

    public Map(string Filename)
    {
        TextAsset textAsset = Resources.Load("level/" + Filename) as TextAsset;
        if (textAsset == null)
        {
            Debug.Log("No such Level: " + Filename);
            return;
        }
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset.text);

        XmlNodeList tiles = xmldoc.SelectNodes("map/tile");
        foreach (XmlNode tile in tiles)
        {
            int x = Int32.Parse(tile.SelectSingleNode("pos_x").InnerText);
            int y = Int32.Parse(tile.SelectSingleNode("pos_y").InnerText);
            tileset.Add(new Pos(x, y),
                new Hexagon(Hexagon.ParseTileType(tile.SelectSingleNode("type").InnerText),
                Hexagon.ParseDirection(tile.SelectSingleNode("dir").InnerText),
                new Pos(x, y), MonoBehaviour.FindObjectOfType<TileHandler>()));
        }

        XmlNode startNode = xmldoc.SelectSingleNode("map/start");
        int start_x = Int32.Parse(startNode.SelectSingleNode("pos_x").InnerText);
        int start_y = Int32.Parse(startNode.SelectSingleNode("pos_y").InnerText);
        start = new KeyValuePair<Pos, Hexagon>(new Pos(start_x, start_y),
            new Hexagon(TileType.Start, Hexagon.ParseDirection(startNode.SelectSingleNode("dir").InnerText), new Pos(start_x, start_y), MonoBehaviour.FindObjectOfType<TileHandler>()));

        XmlNode endNode = xmldoc.SelectSingleNode("map/end");
        int end_x = Int32.Parse(endNode.SelectSingleNode("pos_x").InnerText);
        int end_y = Int32.Parse(endNode.SelectSingleNode("pos_y").InnerText);
        end = new KeyValuePair<Pos, Hexagon>(new Pos(end_x, end_y),
            new Hexagon(TileType.End, Hexagon.ParseDirection(endNode.SelectSingleNode("dir").InnerText), new Pos(end_x, end_y), MonoBehaviour.FindObjectOfType<TileHandler>()));
    }
}

public class Pos
{
    public int x, y;
    /* 좌표 체계
     * 기획서 맵 예시를 기준으로, 
     * 맨 아래 반거울이 (1, 0)
     * 그 왼쪽 위 거울이 (0, 1)
     * 같은 y에 대해 x가 증가하면 ↗↘↗↘식으로 타일 구성
    **/

    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override int GetHashCode()
    {
        return x * 1 << 16 + y;
    }
    public override bool Equals(object obj)
    {
        return x == (obj as Pos).x && y == (obj as Pos).y;
    }
    public bool Equals(Pos obj)
    {
        return x == obj.x && y == obj.y;
    }
}