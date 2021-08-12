using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerManager : MonoBehaviour, ICapabilityes
{
    [SerializeField]
    MenuManager menuManager;
    [SerializeField]
    TextMeshProUGUI powerUpTimer;
    [SerializeField]
    RectTransform PowerUpTimerIndecator;

    AudioSource audioSource;
    ObjectFlashing flashing;

    Touch touch;

    bool isImmune;
    bool isSmallSize;

    float timer;
    float powerUpIterator;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        flashing = GetComponent<ObjectFlashing>();
        isImmune = false;
        isSmallSize = false;
        timer = 0;
        powerUpIterator = 8;
    }

    void Update()
    {
        //for touchpads
        powerUpTimer.text = $"{Mathf.Round(powerUpIterator - timer)} sec.";
        if (Input.touchCount > 0 && timer > 0 && timer < powerUpIterator)
        {
            touch = Input.GetTouch(0);
            powerUpTimer.rectTransform.DOMoveX(touch.position.x, 0.2f);
        }
        else if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            powerUpTimer.rectTransform.DOMoveX(touch.position.x, 0.2f);
        }

        // for mause and test
        if (Input.GetMouseButton(0) && timer > 0 && timer < powerUpIterator)
        {
            powerUpTimer.rectTransform.DOMoveX(Input.mousePosition.x, 0.2f);
        }
        else if (Input.GetMouseButton(0))
        {
            powerUpTimer.rectTransform.DOMoveX(Input.mousePosition.x, 0.2f);
        }

        if (isImmune || isSmallSize)
        {
            timer += Time.deltaTime;
        }

        if (isImmune)
        {
            flashing.StartFlashing();
            if (timer >= powerUpIterator)
            {
                flashing.EndFlashing();
                PowerUpTimerIndecator.DOAnchorPosY(500, 0.6f);
                gameObject.transform.DOScale(new Vector3(0.5f, 0.1f, 0.5f), 0.1f);
                timer = 0;
                isImmune = false;
            }
        }

        if (isSmallSize)
        {
            if (timer >= powerUpIterator)
            {
                PowerUpTimerIndecator.DOAnchorPosY(500, 0.6f);
                gameObject.transform.DOScale(new Vector3(0.5f, 0.1f, 0.5f), 0.1f);
                timer = 0;
                isSmallSize = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PowerUpMove>())
        {
            Destroy(other.gameObject);
            int kindOfPower = Random.Range(0, 2);
            if (kindOfPower == 0)
            {
                ActivateImmunity();
            }
            else
            {
                ActivateSizeChanger();
            }
        }
        else
        {
            OnObstacleEnter(other);
        }
    }

    public void ActivateImmunity()
    {
        isImmune = true;
        timer = 0;
        PowerUpTimerIndecator.DOAnchorPosY(650, 0.1f);
        powerUpTimer.text = $"{Mathf.Round(powerUpIterator)} sec.";
    }

    public void ActivateSizeChanger()
    {
        isSmallSize = true;
        timer = 0;
        gameObject.transform.DOScale(new Vector3(0.3f, 0.1f, 0.3f), 0.1f);
        PowerUpTimerIndecator.DOAnchorPosY(650, 0.1f);
        powerUpTimer.text = $"{Mathf.Round(powerUpIterator)} sec.";
    }

    private void OnObstacleEnter(Collider other)
    {
        if (GameObject.Find("GameArea"))
        {
            GameObject gameArea = GameObject.Find("GameArea");
            ObstacleMove[] obstacles = gameArea.GetComponentsInChildren<ObstacleMove>();
            PowerUpMove[] powerUps = gameArea.GetComponentsInChildren<PowerUpMove>();
            if (isImmune && other.gameObject.GetComponent<ObstacleMove>())
            {
                audioSource.PlayOneShot(audioSource.clip);
                other.gameObject.SetActive(false);
            }
            else
            {
                other.gameObject.GetComponent<ObjectFlashing>().StartFlashing();
                timer = powerUpIterator;

                for (int i = 0; i < obstacles.Length; i++)
                    obstacles[i].enabled = false;

                for (int i = 0; i < powerUps.Length; i++)
                    Destroy(powerUps[i].gameObject);

                gameArea.GetComponent<WayGenerator>().StopAllCoroutines();
                gameArea.GetComponent<WayGenerator>().enabled = false;
                gameArea.GetComponent<ScoreCounter>().enabled = false;
                gameObject.GetComponent<PlayerMovement>().enabled = false;
                menuManager.GameIsOver();
            }
        }
    }
}
