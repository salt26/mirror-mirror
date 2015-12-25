using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map
{
    public Dictionary<Pos, Hexagon> tileset = new Dictionary<Pos, Hexagon>();
    public KeyValuePair<Pos, Direction> start, end; // 시작점, 끝점은 편의상 맵 밖에서 들어오는 인접타일의 좌표로 생각

    public Map()
    {
        // 기획서 맵 예시
        tileset.Add(new Pos(0, 1), new Hexagon(TileType.FullCorner, Direction.NEE));
        tileset.Add(new Pos(0, 2), new Hexagon(TileType.HalfCorner, Direction.SWW));
        tileset.Add(new Pos(0, 3), new Hexagon(TileType.FullEdge, Direction.East));
        tileset.Add(new Pos(1, 0), new Hexagon(TileType.HalfCorner, Direction.North));
        tileset.Add(new Pos(1, 1), new Hexagon(TileType.HalfEdge, Direction.ESS));
        tileset.Add(new Pos(1, 2), new Hexagon(TileType.FullCorner, Direction.SWW));
        tileset.Add(new Pos(1, 3), new Hexagon(TileType.Empty, Direction.Empty));
        tileset.Add(new Pos(2, 1), new Hexagon(TileType.FullCorner, Direction.EES));
        tileset.Add(new Pos(2, 2), new Hexagon(TileType.FullCorner, Direction.NEE));
        tileset.Add(new Pos(2, 3), new Hexagon(TileType.FullEdge, Direction.NNE));
        tileset.Add(new Pos(3, 1), new Hexagon(TileType.Empty, Direction.Empty));
        tileset.Add(new Pos(3, 2), new Hexagon(TileType.HalfCorner, Direction.SWW));

        start = new KeyValuePair<Pos, Direction>(new Pos(1, -1), Direction.North);
        end = new KeyValuePair<Pos, Direction>(new Pos(4, 3), Direction.NEE);
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
        return base.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj as Pos);
    }
    public bool Equals(Pos obj)
    {
        return x == obj.x && y == obj.y;
    }
}