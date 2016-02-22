using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
    public GameObject[] trash;

    void Start()
    {
        foreach(GameObject obj in trash)
        {
            Destroy(obj);
        }
    }

    void Update()
    {

    }
}
