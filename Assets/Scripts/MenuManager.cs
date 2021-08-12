using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] 
    GameObject ResultPanel;
    
    float timer;
    bool IsGameOver;


    void Start()
    {
        IsGameOver = false;
        timer = 0;
    }

    void Update()
    {
        if (IsGameOver)
            timer += Time.deltaTime;

        if (timer > 2)
        {
            ResultPanel.SetActive(true);
            timer = 0;
            IsGameOver = false;
        }
    }

    public void GameIsOver() 
    {
        IsGameOver = true;
    }
}
