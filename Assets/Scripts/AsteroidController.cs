using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        //Настраиваем Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        //Если игрок не умер - вращаем астероид
        if (GameManager.Instance.PlayerTransform != null)
            transform.rotation = Quaternion.Euler(0f, 0f, GameManager.CalculateRotation(GameManager.Instance.PlayerTransform.position, transform.position));
        //Задаём скорость в переменную по рассчётам с GameManager
        float speed = GameManager.CalculateVelocity(tag);
        //Если игрок не умер - задаём направление и скорость
        if (GameManager.Instance.PlayerTransform != null)
            rb.velocity = GameManager.NormalizedDistance(GameManager.Instance.PlayerTransform.position, transform.position) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //..смотрим какого размера был астероид и спавним астероиды дочерние
        switch (tag)
        {
            case "Asteroid":
                break;
            case "MediumAsteroid":
                SpawnSmallAsteroids(collision);
                break;
            case "BigAsteroid":
                SpawnSmallAsteroids(collision);
                SpawnSmallAsteroids(collision);
                break;
            case "HugeAsteroid":
                SpawnSmallAsteroids(collision);
                SpawnSmallAsteroids(collision);
                SpawnSmallAsteroids(collision);
                break;
        }
        //Если столкнулся с игроком...
        if (collision.tag == "Player")
        {
            //Спавн взрыва
            GameManager.SpawnExplosion(gameObject, collision.gameObject, 0);
        }
    }

    private void SpawnSmallAsteroids(Collider2D collision)
    {
        //Спавн маленького астероида в взрывающемся астероиде
        GameObject local = GameManager.SpawnAsteroid(collision.transform);
        //Задаём случайные скорость и направление
        Rigidbody2D localRB = local.GetComponent<AsteroidController>().rb;
        localRB = local.GetComponent<Rigidbody2D>();
        localRB.velocity = new Vector2(Random.Range(-1f, 1f) , Random.Range(-1f, 1f)) * GameManager.Instance.AsteroidSpeed;
        local.GetComponent<AsteroidController>().rb = localRB;
    }
}