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
        //Если столкнулся с игроком...
        if (collision.tag == "Player")
        {
            //Спавн взрыва
            GameManager.SpawnExplosion(gameObject, collision.gameObject, 0);
        }
    }
}