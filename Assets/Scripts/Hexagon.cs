﻿using UnityEngine;
using System.Collections;

public class Hexagon
{
    public TileType tile;
    public Direction dir;
    public GameObject obj;

    public Hexagon(TileType tile, Direction dir, Pos p)
    {
        this.tile = tile;
        this.dir = dir;
        obj = MonoBehaviour.Instantiate(MonoBehaviour.FindObjectOfType<GameLoader>().tiles[(int)tile],
                InputHandler.PosToWorld(p), Quaternion.AngleAxis(DirectionToDegree(dir), Vector3.back)) as GameObject;
    }

    public Direction Reflect(Direction input)
    {
        switch (tile)
        {
            case TileType.FullCorner:
            case TileType.FullEdge:
                return DegreeToDirection(DirectionToDegree(dir) * 2 - DirectionToDegree(input));
            case TileType.HalfCorner:
            case TileType.HalfEdge:
                if (((DirectionToDegree(input) > DirectionToDegree(dir) - 90 && DirectionToDegree(dir) > 90) ||
                    (DirectionToDegree(input) > AdjustDegree(DirectionToDegree(dir) - 90) && DirectionToDegree(dir) < 90)) &&
                    ((DirectionToDegree(input) < DirectionToDegree(dir) + 90 && DirectionToDegree(dir) < 270) ||
                    (DirectionToDegree(input) < AdjustDegree(DirectionToDegree(dir) + 90) && DirectionToDegree(dir) > 270)))
                    return DegreeToDirection(DirectionToDegree(dir) * 2 - DirectionToDegree(input));
                else
                    return input;
            case TileType.Empty:
            default:
                return input;
        }
    }

    public void Flip(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
            case Direction.NEE:
            case Direction.EES:
            case Direction.South:
            case Direction.SWW:
            case Direction.WWN:
                this.dir = DegreeToDirection(DirectionToDegree(dir) * 2 - DirectionToDegree(this.dir));
                return;
            default:
                return;
        }
    }

    public static int AdjustDegree(int degree)
    {
        int result = degree;
        result = (result % 360);    // Over 360
        result = (result < 0) ? 360 - ((-result) % 360) : result; // Under 0
        return result;
    }

    public static Direction DegreeToDirection(int degree)
    {
        switch (AdjustDegree(degree))
        {
            case 0:
                return Direction.North;
            case 30:
                return Direction.NNE;
            case 60:
                return Direction.NEE;
            case 90:
                return Direction.East;
            case 120:
                return Direction.EES;
            case 150:
                return Direction.ESS;
            case 180:
                return Direction.South;
            case 210:
                return Direction.SSW;
            case 240:
                return Direction.SWW;
            case 270:
                return Direction.West;
            case 300:
                return Direction.WWN;
            case 330:
                return Direction.WNN;
            default:
                return Direction.Empty;
        }
    }

    public static int DirectionToDegree(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return 0;
            case Direction.NNE:
                return 30;
            case Direction.NEE:
                return 60;
            case Direction.East:
                return 90;
            case Direction.EES:
                return 120;
            case Direction.ESS:
                return 150;
            case Direction.South:
                return 180;
            case Direction.SSW:
                return 210;
            case Direction.SWW:
                return 240;
            case Direction.West:
                return 270;
            case Direction.WWN:
                return 300;
            case Direction.WNN:
                return 330;
            default:
                return 360;
        }
    }

    public static Pos NextTile(Pos p, Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return new Pos(p.x, p.y + 1);
            case Direction.NEE:
                return new Pos(p.x + 1, (p.x % 2 == 0) ? p.y : p.y + 1);
            case Direction.EES:
                return new Pos(p.x + 1, (p.x % 2 == 0) ? p.y - 1 : p.y);
            case Direction.South:
                return new Pos(p.x, p.y - 1);
            case Direction.SWW:
                return new Pos(p.x - 1, (p.x % 2 == 0) ? p.y - 1 : p.y);
            case Direction.WWN:
                return new Pos(p.x - 1, (p.x % 2 == 0) ? p.y : p.y + 1);
            default:
                Debug.Log("Invalid Direction");
                return p;
        }
    }
}

public enum TileType { Empty, HalfCorner, HalfEdge, FullCorner, FullEdge }  // Half : 반거울, Full : 거울 / Corner : 꼭지점을 잇는 거울, Edge : 변의 중심을 잇는 거울
public enum Direction
{
    Empty = 0,
    North,
    NNE,
    NEE,
    East,
    EES,
    ESS,
    South,
    SSW,
    SWW,
    West,
    WWN,
    WNN
}
