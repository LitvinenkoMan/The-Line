using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerUpFlashing : MonoBehaviour
{

    MeshRenderer powerUpMaterial;
    float duration;
    float timer;

    void Start()
    {
        powerUpMaterial = gameObject.GetComponent<MeshRenderer>();
        duration = 0.6f;
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            powerUpMaterial.material.DOColor(GiveColor(), duration);
            powerUpMaterial.material.color = GiveColor();
            timer = 0;
        }
    }

    private Color GiveColor()
    {
        float color = Random.Range(0, 7);
        switch (color)
        {
            case 0: return Color.red; break;
            case 1: return Color.black; break;
            case 2: return Color.yellow; break;
            case 3: return Color.green; break;
            case 4: return Color.blue; break;
            case 5: return Color.white; break;
            default: return Color.cyan; break;
        }
    }
}
