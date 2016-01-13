using UnityEngine;
using System.Collections;

public class Hexagon
{
    public TileType tile;
    public Direction dir;
    public GameObject obj;

    public Hexagon(TileType tile, Direction dir, Pos p, TileHandler t)
    {
        this.tile = tile;
        this.dir = dir;
        obj = MonoBehaviour.Instantiate(t.tiles[(int)tile],
                Transformer.PosToWorld(p), Quaternion.AngleAxis(DirectionToDegree(dir), Vector3.back)) as GameObject;
    }

    public Direction Reflect(Direction input)
    {
        switch (tile)
        {
            case TileType.FullCorner:
            case TileType.FullEdge:
                return DegreeToDirection(DirectionToDegree(dir) * 2 - DirectionToDegree(input) + 180);
            case TileType.HalfCorner:
            case TileType.HalfEdge:
                if (DegreeBetween(DirectionToDegree(input), DirectionToDegree(dir)) < 90)
                    return input;
                else
                    return DegreeToDirection(DirectionToDegree(dir) * 2 - DirectionToDegree(input) + 180);
            case TileType.Empty:
            default:
                return input;
        }
    }

    public static int DegreeBetween(int deg1, int deg2)
    {
        int diff = Mathf.Abs(AdjustDegree(deg1 - deg2));
        if (diff > 180) diff = 360 - diff;
        return diff;
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
                Direction originalDir = this.dir;
                this.dir = DegreeToDirection(DirectionToDegree(dir) * 2 - DirectionToDegree(originalDir) + 180);
                obj.transform.rotation = Quaternion.AngleAxis(DirectionToDegree(this.dir), Vector3.back);
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

    public static TileType ParseTileType(string str)
    {
        if (str.Equals("Empty")) return TileType.Empty;
        else if (str.Equals("HalfCorner")) return TileType.HalfCorner;
        else if (str.Equals("HalfEdge")) return TileType.HalfEdge;
        else if (str.Equals("FullCorner")) return TileType.FullCorner;
        else if (str.Equals("FullEdge")) return TileType.FullEdge;
        else {
            Debug.LogError("Wrong Input : ParseTileType");
            return TileType.Empty;
        }
    }

    public static Direction ParseDirection(string str)
    {
        if (str.Equals("Empty")) return Direction.Empty;
        else if (str.Equals("North")) return Direction.North;
        else if (str.Equals("NNE")) return Direction.NNE;
        else if (str.Equals("NEE")) return Direction.NEE;
        else if (str.Equals("East")) return Direction.East;
        else if (str.Equals("EES")) return Direction.EES;
        else if (str.Equals("ESS")) return Direction.ESS;
        else if (str.Equals("South")) return Direction.South;
        else if (str.Equals("SSW")) return Direction.SSW;
        else if (str.Equals("SWW")) return Direction.SWW;
        else if (str.Equals("West")) return Direction.West;
        else if (str.Equals("WWN")) return Direction.WWN;
        else if (str.Equals("WNN")) return Direction.WNN;
        else
        {
            Debug.LogError("Wrong Input : ParseDirection");
            return Direction.Empty;
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
