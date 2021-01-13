using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //Применение паттерна Одиночка (Singletone)
    public static GameManager Instance;

    //Разные настройки и важные параметры, доступ к которым необходим в других классах
    public bool GameIsOver = false;
    public int Score;
    public float AsteroidSpeed = 2f;
    public float BulletSpeed = 60f;
    public float RespawnTime = 2f;
    public Text ScoreText;
    public Transform CursorArt;
    public Transform GunPoint;
    public Transform PlayerTransform;
    public GameObject BulletPrefab;
    public GameObject ExplosionPrefab;
    public GameObject PlayerPrefab;
    public GameObject GameIsOnPanel;
    public GameObject GameIsOverPanel;
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;
    public List<GameObject> AsteroidPrefabs;
    public List<GameObject> MediumAsteroidPrefabs;
    public List<GameObject> BigAsteroidPrefabs;
    public List<GameObject> HugeAsteroidPrefabs;

    [HideInInspector] public Vector2 ScreenBounds;

    private void Awake()
    {
        //Это обеспечивает паттерн Одиночку.
        if (Instance == null)
            Instance = this;
            //Если бы сцен было больше - то нужно было бы добавить DontDestroyOnLoad(this);
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //Сохраняем границы экрана
        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.GetComponent<Transform>().position.z));
        //Сохраняем координаты игрока
        PlayerTransform = GameObject.Find("Player").transform;
        //Запускаем астероиды
        Instance.StartCoroutine(AsteroidWave());
    }

    private void Update()
    {
        //Проверка на проигрыш
        if (PlayerTransform == null)
        {
            Die();
        }
    }

    public static void StartGame()
    {
        //Запускаем игрока
        SpawnPlayer();
        if (Instance.GameIsOver)
        {
            //Если совсем проиграли - заново грузим сцену
            SceneManager.LoadScene(0);
        }
        //Подготавливаем игру к запуску (настройки интерфейса)
        Instance.GameIsOver = false;
        Instance.GameIsOverPanel.SetActive(false);
        Instance.GameIsOnPanel.SetActive(true);
        Cursor.visible = false;
    }

    public static void GameOver()
    {
        //Настройки интерфейса при конце игры
        Instance.GameIsOver = true;
        Instance.GameIsOverPanel.SetActive(true);
        Instance.GameIsOnPanel.SetActive(false);
        Cursor.visible = true;
        Instance.Score = 0;
        UpdateScore(Instance.Score);
    }

    public static float CalculateRotation(Vector3 target, Vector3 rotateable)
    {
        //Векторная математика - находим насколько градусов надо повернуть rotateable, чтоб он смотрел в сторону target
        //Возвращает колличество градусов
        Vector3 difference = target - rotateable;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        return rotationZ;
    }

    public static Vector3 NormalizedDistance(Vector3 target, Vector3 rotateable)
    {
        //Возвращает нормализованное направление до цели (длина вектора == 1)
        Vector3 local = target - rotateable;
        local.Normalize();
        return local;
    }

    public static float CalculateVelocity(string tag)
    {
        //В зависимости от размера астероида - разнится скорость
        switch (tag)
        {
            case "Asteroid":
                return Instance.AsteroidSpeed * 1;
            case "MediumAsteroid":
                return Instance.AsteroidSpeed * 0.7f;
            case "BigAsteroid":
                return Instance.AsteroidSpeed * 0.4f;
            case "HugeAsteroid":
                return Instance.AsteroidSpeed * 0.1f;
        }
        return 0;
    }

    public static void SpawnExplosion(GameObject first, GameObject second, int score)
    {
        //Запускаем взрыв и изменяем Очки, скорость и прочее.
        Instance.Score += score;
        UpdateScore(Instance.Score);
        Instance.AsteroidSpeed += 0.05f;
        Instance.RespawnTime -= 0.05f;
        //Следим чтобы переменная не ушла ниже нуля и не превратила игру в ад из астероидов
        Instance.RespawnTime = Mathf.Clamp(Instance.RespawnTime, 1f, 2f);
        GameObject explosion = Instantiate(Instance.ExplosionPrefab) as GameObject;
        explosion.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
        explosion.transform.position = first.transform.position;
        //Удаляем столкнувшиеся объекты
        Destroy(first);
        Destroy(second);
    }

    public static void SpawnPlayer()
    {
        //Спавн игрока
        GameObject local = Instantiate(Instance.PlayerPrefab, Vector3.zero, Quaternion.identity);
        Instance.PlayerTransform = local.transform;
        Instance.GunPoint = Instance.PlayerTransform.GetChild(0).GetChild(1);
    }

    public static void Die()
    {
        //Если есть сердца - играем дальше. Если нет - конец игры
        if (!Instance.Heart1.activeSelf)
        {
            if (!Instance.Heart2.activeSelf)
            {
                if (!Instance.Heart3.activeSelf)
                {
                    GameOver();
                }
                else
                {
                    Instance.Heart3.SetActive(false);
                    StartGame();
                    if (Instance.PlayerTransform == null)
                        SpawnPlayer();
                    return;
                }
            }
            else
            {
                Instance.Heart2.SetActive(false);
                StartGame();
                if (Instance.PlayerTransform == null)
                    SpawnPlayer();
                return;
            }
        }
        else
        {
            Instance.Heart1.SetActive(false);
            StartGame();
            if (Instance.PlayerTransform == null)
                SpawnPlayer();
            return;
        }
    }

    public static void UpdateScore(int score)
    {
        //Обновляет отображение очков
        Instance.ScoreText.text = $"Score: {score}";
    }

    public static IEnumerator AsteroidWave()
    {
        //Запускает астероиды вне экрана в случайных сторонах
        while (!Instance.GameIsOver)
        {
            yield return new WaitForSeconds(Instance.RespawnTime);
            int local = Random.Range(0, 4);
            switch (local)
            {
                case 0:
                    GameObject localAsteroid0 = SpawnAsteroid();
                    localAsteroid0.transform.position = new Vector2(Instance.ScreenBounds.x * -2, Random.Range(-Instance.ScreenBounds.y, Instance.ScreenBounds.y));
                    break;
                case 1:
                    GameObject localAsteroid1 = SpawnAsteroid();
                    localAsteroid1.transform.position = new Vector2(Instance.ScreenBounds.x * 2, Random.Range(-Instance.ScreenBounds.y, Instance.ScreenBounds.y));
                    break;
                case 2:
                    GameObject localAsteroid2 = SpawnAsteroid();
                    localAsteroid2.transform.position = new Vector2(Random.Range(-Instance.ScreenBounds.x, Instance.ScreenBounds.x), Instance.ScreenBounds.y * -2);
                    break;
                case 3:
                    GameObject localAsteroid3 = SpawnAsteroid();
                    localAsteroid3.transform.position = new Vector2(Random.Range(-Instance.ScreenBounds.x, Instance.ScreenBounds.x), Instance.ScreenBounds.y * 2);
                    break;
            }
        }
    }

    public static GameObject SpawnAsteroid()
    {
        //В зависимости от текущей скорости спавнит нужный размер астероида
        if (Instance.AsteroidSpeed < 6)
        {
            GameObject local = Instantiate(Instance.AsteroidPrefabs[Random.Range(0, Instance.AsteroidPrefabs.Count)]) as GameObject;
            return local;
        }
        else if (Instance.AsteroidSpeed < 8)
        {
            GameObject local = Instantiate(Instance.MediumAsteroidPrefabs[Random.Range(0, Instance.MediumAsteroidPrefabs.Count)]) as GameObject;
            return local;
        }
        else if (Instance.AsteroidSpeed < 10)
        {
            GameObject local = Instantiate(Instance.BigAsteroidPrefabs[Random.Range(0, Instance.BigAsteroidPrefabs.Count)]) as GameObject;
            return local;
        }
        else
        {
            GameObject local = Instantiate(Instance.HugeAsteroidPrefabs[Random.Range(0, Instance.HugeAsteroidPrefabs.Count)]) as GameObject;
            return local;
        }
    }

    public static GameObject SpawnAsteroid(Transform localTransform)
    {
        //Спавн астероида в определённых координатах
        GameObject local = Instantiate(Instance.AsteroidPrefabs[Random.Range(0, Instance.AsteroidPrefabs.Count)], localTransform.position, Quaternion.identity) as GameObject;
        return local;
    }

    public static GameObject SpawnBullet()
    {
        //Свавн пули
        GameObject local = Instantiate(Instance.BulletPrefab, Instance.GunPoint.position, Quaternion.identity) as GameObject;
        return local;
    }
}
