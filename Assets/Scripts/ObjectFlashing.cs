using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectFlashing : MonoBehaviour
{
    Material objectMaterial;
    Color startColor;
    public float Duration;
    float timer;
    bool isFlashing;

    void Start()
    {
        objectMaterial = gameObject.GetComponent<MeshRenderer>().material;
        startColor = objectMaterial.color;
        if (Duration == 0)
            Duration = 0.5f;
        isFlashing = false;
        timer = 0;
    }

    void Update()
    {
        if (isFlashing)
        {
            timer += Time.deltaTime;
            if (timer > Duration)
            {
                if (objectMaterial.color == startColor)
                {
                    objectMaterial.DOColor(Color.yellow, Duration);
                }
                else
                {
                    objectMaterial.DOColor(startColor, Duration);
                }
                timer = 0;
            }
        }
    }

    public void StartFlashing()
    {
        isFlashing = true;
    }

    public void EndFlashing()
    {
        isFlashing = false;
        timer = 0;
        objectMaterial.DOColor(startColor, Duration);
    }
}
