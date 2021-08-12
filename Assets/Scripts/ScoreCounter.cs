using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Score;
    [SerializeField]
    TextMeshProUGUI GameOverScore;
    [SerializeField]
    TextMeshProUGUI BestScore;

    float timer;
    
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Score.text = $"{Mathf.Round(timer * 10)}";

        GameOverScore.text = Score.text;

        if (int.Parse(Score.text) > int.Parse(BestScore.text))
            BestScore.text = Score.text;
    }

    public void Restart() 
    {
        timer = 0;
        Score.text = "0";
        GameOverScore.text = "0";
    }


}
