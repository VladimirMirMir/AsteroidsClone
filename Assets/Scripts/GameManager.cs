using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Score;
    public float AsteroidSpeed = 2f;
    public Transform CursorArt;
    public GameObject ExplosionPrefab;
    public List<GameObject> AsteroidPrefabs;
    public List<GameObject> MediumAsteroidPrefabs;
    public List<GameObject> BigAsteroidPrefabs;
    public List<GameObject> HugeAsteroidPrefabs;

    [HideInInspector] public Vector2 ScreenBounds;
    [HideInInspector] public Transform PlayerTransform;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.GetComponent<Transform>().position.z));
        PlayerTransform = GameObject.Find("Player").transform;
    }

    public float CalculateRotation(Vector3 target, Vector3 rotateable)
    {
        Vector3 difference = target - rotateable;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        return rotationZ;
    }

    public static float CalculateVelocity(string tag)
    {
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

    public void SpawnExplosion(GameObject first, GameObject second, int score)
    {
        Score += score;
        GameObject explosion = Instantiate(ExplosionPrefab) as GameObject;
        explosion.transform.position = first.transform.position;
        Destroy(first);
        Destroy(second);
    }
}
