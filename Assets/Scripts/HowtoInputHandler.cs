using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HowtoInputHandler : MonoBehaviour
{
    Vector3 clickedPos;
    int pos = 0;
    public int maxPage;
    public Text pageStatus;
    public Text title;
    string[] titleList = { "Intro", "Goal", "Flip", "Mirror", "Half Mirror", "Camera" };

    void Start()
    {

    }

    void Update()
    {
        if (pos < 0)
        {
            pageStatus.text = 1 + " / " + maxPage;
        }
        else if (pos >= maxPage)
        {
            pageStatus.text = maxPage + " / " + maxPage;
        }
        else
        {
            pageStatus.text = (pos + 1) + " / " + maxPage;
        }

        title.text = titleList[pos];

        if (Input.GetMouseButtonDown(0))
        {
            clickedPos = Input.mousePosition;
        }

        else if (Input.GetMouseButton(0))
        {
            Camera.main.transform.position = new Vector3(4.8f * pos + (Camera.main.ScreenToWorldPoint(clickedPos) - Camera.main.ScreenToWorldPoint(Input.mousePosition)).x, Camera.main.transform.position.y, -10f);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            if ((Input.mousePosition - clickedPos).x < -120f)
            {
                if (pos < maxPage - 1)
                {
                    StartCoroutine(slideRight());
                }
                else
                {
                    StartCoroutine(slideLeft());
                    pos++;
                }
            }
            else if ((Input.mousePosition - clickedPos).x < 0f)
            {
                StartCoroutine(slideLeft());
                pos++;
            }

            if ((Input.mousePosition - clickedPos).x > 120f)
            {
                if (pos > 0)
                {
                    StartCoroutine(slideLeft());
                }
                else
                {
                    StartCoroutine(slideRight());
                    pos--;
                }
            }
            else if ((Input.mousePosition - clickedPos).x > 0f)
            {
                StartCoroutine(slideRight());
                pos--;
            }
        }
    }

    IEnumerator slideRight()
    {
        for (float x = 4.8f * pos + (Camera.main.ScreenToWorldPoint(clickedPos) - Camera.main.ScreenToWorldPoint(Input.mousePosition)).x; x < 4.8f * (pos + 1); x += Time.deltaTime * 10f)
        {
            Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, -10f);
            yield return null;
        }
        pos++;
        Camera.main.transform.position = new Vector3(4.8f * pos, Camera.main.transform.position.y, -10f);
    }

    IEnumerator slideLeft()
    {
        for (float x = 4.8f * pos + (Camera.main.ScreenToWorldPoint(clickedPos) - Camera.main.ScreenToWorldPoint(Input.mousePosition)).x; x > 4.8f * (pos - 1); x -= Time.deltaTime * 10f)
        {
            Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, -10f);
            yield return null;
        }
        pos--;
        Camera.main.transform.position = new Vector3(4.8f * pos, Camera.main.transform.position.y, -10f);
    }

    public void onLeftClick()
    {
        if (pos > 0)
        {
            StartCoroutine(slideLeft());
        }
        Camera.main.transform.position = new Vector3(4.8f * pos, Camera.main.transform.position.y, -10f);
    }

    public void onRightClick()
    {
        if (pos < maxPage - 1)
        {
            StartCoroutine(slideRight());
        }
        Camera.main.transform.position = new Vector3(4.8f * pos, Camera.main.transform.position.y, -10f);
    }
}