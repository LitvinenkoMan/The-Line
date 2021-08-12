using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField] private GameObject poolObj;
    [SerializeField] private Transform poolObjPosition;
    private float timer;
    private float timer2;

    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.00001f)
        {
            CreateObj();
            timer = 0;
            Debug.Log($"Timer  {++timer2}");
        }

    }

    void CreateObj()
    {
        if (pool.Count < 1 || pool.Peek().activeSelf || pool.Count < 50)
        {
            GameObject p = Instantiate(poolObj, poolObjPosition);

            pool.Enqueue(p);
        }
        else
        {
            pool.Peek().transform.position = poolObjPosition.position;
            pool.Peek().SetActive(true);

            pool.Enqueue(pool.Peek());
        }
    }
}
