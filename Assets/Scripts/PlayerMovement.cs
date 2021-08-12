using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class PlayerMovement : MonoBehaviour, IMoveable
{
    public bool CanMove = true;

    [Inject]
    ObstacleMove obstacle;

    public void Move()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            gameObject.transform.DOMoveX(ray.origin.x, 0, false);
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction);
            gameObject.transform.DOMoveX(ray.origin.x, 0, false);
        }
    }

    void Update()
    {
        if (CanMove)
        {
            Move();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == obstacle)
        {
            Debug.Log("!!!");
        }
    }

    public void Restart()
    {
        gameObject.transform.DOMoveX(0, 0, false);
        gameObject.transform.DOScale(new Vector3(0.5f, 0.1f, 0.5f), 0);
    }
}
