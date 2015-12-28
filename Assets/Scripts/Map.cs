using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map
{
    public Dictionary<Pos, Hexagon> tileset = new Dictionary<Pos, Hexagon>();
    public KeyValuePair<Pos, Direction> start, end;

    public Map()
    {
        // 기획서 맵 예시
        tileset.Add(new Pos(0, 1), new Hexagon(TileType.FullCorner, Direction.NEE, new Pos(0, 1)));
        tileset.Add(new Pos(0, 2), new Hexagon(TileType.HalfCorner, Direction.SWW, new Pos(0, 2)));
        tileset.Add(new Pos(0, 3), new Hexagon(TileType.FullEdge, Direction.East, new Pos(0, 3)));
        tileset.Add(new Pos(1, 0), new Hexagon(TileType.HalfCorner, Direction.North, new Pos(1, 0)));
        tileset.Add(new Pos(1, 1), new Hexagon(TileType.HalfEdge, Direction.ESS, new Pos(1, 1)));
        tileset.Add(new Pos(1, 2), new Hexagon(TileType.FullCorner, Direction.SWW, new Pos(1, 2)));
        tileset.Add(new Pos(1, 3), new Hexagon(TileType.Empty, Direction.Empty, new Pos(1, 3)));
        tileset.Add(new Pos(2, 1), new Hexagon(TileType.FullCorner, Direction.EES, new Pos(2, 1)));
        tileset.Add(new Pos(2, 2), new Hexagon(TileType.FullCorner, Direction.NEE, new Pos(2, 2)));
        tileset.Add(new Pos(2, 3), new Hexagon(TileType.FullEdge, Direction.NNE, new Pos(2, 3)));
        tileset.Add(new Pos(3, 1), new Hexagon(TileType.Empty, Direction.Empty, new Pos(3, 1)));
        tileset.Add(new Pos(3, 2), new Hexagon(TileType.HalfCorner, Direction.SWW, new Pos(3, 2)));

        start = new KeyValuePair<Pos, Direction>(new Pos(1, -1), Direction.North);
        end = new KeyValuePair<Pos, Direction>(new Pos(3, 2), Direction.NEE);
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