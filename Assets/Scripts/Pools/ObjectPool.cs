using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject objPrefab;
    private Transform container;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public ObjectPool(GameObject prefab, int size, Transform box)
    {
        objPrefab = prefab;
        container = box;

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Object.Instantiate(objPrefab, container);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Object.Instantiate(objPrefab, container);
        obj.SetActive(true);

        return obj;
    }

    public void Restore(GameObject obj)
    {
        obj.GetComponent<IResettable>()?.Reset();

        obj.SetActive(false);
        obj.transform.SetParent(container);
        pool.Enqueue(obj);
    }
}
