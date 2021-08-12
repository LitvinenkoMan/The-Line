using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleMove : MonoBehaviour, IMoveable
{
    public bool CanMove = false;
    public void Move()
    {
        transform.DOMove(transform.position - Vector3.forward, 1f, false);
    }
    private void Awake()
    {
        CanMove = true;
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            Move();
        }
    }
}
