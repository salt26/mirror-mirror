using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartSceneButtonHandler : MonoBehaviour
{
    public GameObject[] tiles;
    public float flipTime = 0.5f; // Flip animation에 걸리는 시간

    bool isFlipping = false;

    void Start()
    {
    }

    void Update()
    {
        if (!isFlipping)
        {
            foreach (GameObject tile in tiles)
            {
                StartCoroutine(Flip(tile, Direction.SWW));
            }
            isFlipping = true;
        }
    }

    public void onGameStartClick()
    {
        SceneManager.LoadScene("map_select_final");
    }

    public void onExitClick()
    {
        Application.Quit();
    }

    public void onHowtoClick()
    {
        SceneManager.LoadScene("howtoplay");
    }

    IEnumerator Flip(GameObject tile, Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
            case Direction.NEE:
            case Direction.EES:
            case Direction.South:
            case Direction.SWW:
            case Direction.WWN:
                Direction originalDir = tile.GetComponent<StartSceneTile>().dir;
                tile.GetComponent<StartSceneTile>().dir = Hexagon.DegreeToDirection(Hexagon.DirectionToDegree(dir) * 2 - Hexagon.DirectionToDegree(originalDir) + 180);
                int axisdig = (Hexagon.DirectionToDegree(tile.GetComponent<StartSceneTile>().dir) - Hexagon.DirectionToDegree(originalDir) + 360) / 2 + Hexagon.DirectionToDegree(originalDir);
                //int axisdig = Hexagon.DirectionToDegree(dir) + 90;
                Debug.Log(Hexagon.DirectionToDegree(originalDir) + "->" + Hexagon.DirectionToDegree(tile.GetComponent<StartSceneTile>().dir) + " : " + axisdig);
                float rotatesum = 0;
                while (rotatesum < 180)
                {
                    tile.transform.Rotate(new Vector3(Mathf.Sin(Mathf.Deg2Rad * axisdig), Mathf.Cos(Mathf.Deg2Rad * axisdig)) * Time.deltaTime * 180 / flipTime, Space.World);
                    rotatesum += Time.deltaTime * 180 / flipTime;
                    yield return null;
                }
                tile.transform.rotation = Quaternion.AngleAxis(Hexagon.DirectionToDegree(tile.GetComponent<StartSceneTile>().dir), Vector3.back);
                yield return new WaitForSeconds(flipTime);
                break;
            default:
                break;
        }
        isFlipping = false;
    }
}