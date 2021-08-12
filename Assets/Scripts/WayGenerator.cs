using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WayGenerator : MonoBehaviour
{
    Queue<GameObject> pool = new Queue<GameObject>();
    public int CountofObjects;

    [SerializeField] GameObject Obstacle;
    [SerializeField] GameObject PowerUp;
    float timer, iterator;
    int freeSpace;
    int[] way = { 0, 1, 1, 1, 1, 1, 0 };

    private void OnEnable()
    {
        freeSpace = 3;
        iterator = 0.36f;
        timer = 0;
        StartCoroutine(NextStepBuilder());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > iterator)
        {
            for (int i = 0; i < 7; i++)
            {
                if (way[i] == 0)
                {
                    CreateObj(Obstacle, new Vector3(i - 3, 0, 10));
                }
                else if (way[i] == 2)
                {
                    GameObject newPowerUp = Instantiate(PowerUp, new Vector3(i - 3, 0, 10), PowerUp.gameObject.transform.rotation);
                    newPowerUp.transform.SetParent(gameObject.transform);
                    newPowerUp.SetActive(true);
                    Destroy(newPowerUp, 10f);
                }
            }

            timer = 0;
        }
    }

    void CreateObj(GameObject ObjectToCreate, Vector3 position)                         //паттерн Object pool с использованием очереди
    {
        if (pool.Count < 1 || pool.Peek().activeSelf || pool.Count < 150)         //если кол-во объектов меньше 150 или самый первый из них активен
        {
            GameObject newObject = Instantiate(ObjectToCreate);     // то создаём
            newObject.transform.position = position;
            newObject.transform.SetParent(gameObject.transform);
            pool.Enqueue(newObject);
        }
        else                                                        // иначе берём самый первый
        {
            pool.Peek().transform.position = position;
            pool.Peek().GetComponent<ObstacleMove>().enabled = true;
            pool.Peek().SetActive(true);
            pool.Enqueue(pool.Dequeue());               //убераем его из конца очереди и запихиваем в начало
        }
    }

    IEnumerator NextStepBuilder()
    {
        float timer = 0;
        int swerve = 0, steps = 0, firstSteps = 0;

        while (true)
        {
            timer += Time.deltaTime;
            // 0 - obstacle, 1 - free space, 2 - power up
            if (timer > iterator)
            {
                if (firstSteps < 8)                                                                          //отвечает за начальную заставку в каждой новой игре
                {                                                                                           
                    switch (firstSteps)
                    {                                                                                       // {0 1 1 1 1 1 0}
                        case 2: way[1] = 0; way[5] = 0; break;                                              // {0 0 1 1 1 0 0}
                        case 4: way[2] = 0; way[4] = 0; break;                                              // {0 0 0 1 0 0 0}
                        default:
                            break;
                    }
                    firstSteps++;
                }
                else if (steps <= 0) //если шагов не осталось, создаём отклонение от основного пути         //данная куратина строит путь используя одномерный массив
                {                                                                                           //где каждые iterator секунд меняет значения в зависимости от оставшихся шагов (steps)
                    if (swerve < freeSpace)                                                                 //есть переменная freeSpace, которая указывает, какая позиция сейчас свободна, если freeSpace = 2
                    {                                                                                       //то массив соответственно будет {0 0 1 0 0 0 0}, swerve - отклонение от основного пути, он устанавливается рандомно (предположим swerve = 5),
                        for (int i = swerve; i < freeSpace + 1; i++)                                        //после чего осуществляется путь {0 0 1 1 1 1 0}, и freeSpace становится равен swerve и на этом же этапе определяется количество 
                        {                                                                                   //шагов до следующего отклонения {0 0 0 0 0 1 0} в таком же духе продолжается покаигрок не ошибётся     
                            way[i] = 1;
                        }
                        freeSpace = swerve;
                    }
                    else if (swerve > freeSpace)
                    {
                        for (int i = swerve; i > freeSpace - 1; i--)
                        {
                            way[i] = 1;
                        }
                        freeSpace = swerve;
                    }
                    swerve = Random.Range(0, 7);
                    steps = Random.Range(3, 7);
                }
                else
                {
                    for (int i = 0; i < 7; i++)                                                             //на данном этапе рассчитывается, будет ли на свободном от препятствия месте размещён усилитель
                    {
                        if (i == freeSpace)
                        {
                            way[i] = 1;
                            int chanceOfPowerUp = Random.Range(1, 101);
                            if (chanceOfPowerUp < 4)
                                way[i] = 2;
                        }
                        else
                        {
                            way[i] = 0;
                        }
                    }
                }
                steps--;                                                                                    //итерация прошла, один шаг пройден
                timer = 0;
            }
            yield return null;
        }
    }

    public void Restart()
    {
        ObjectFlashing[] objectFlashings = gameObject.GetComponentsInChildren<ObjectFlashing>();
        for (int i = 0; i < objectFlashings.Length; i++)
            objectFlashings[i].EndFlashing();
        for (int i = 0; i < way.Length; i++)
        {
            if (i == 0 || i == way.Length - 1)
                way[i] = 0;
            else way[i] = 1;
        }

        foreach (var item in pool)
        {
            Destroy(item);
        }
        pool.Clear();

        freeSpace = 3;
        iterator = 0.36f;
        timer = 0;
    }

}
